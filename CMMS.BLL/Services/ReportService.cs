using CMMS.BLL.Helpers;
using CMMS.BLL.Interfaces;
using CMMS.BLL.Mappings;
using CMMS.BLL.Realtime;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Reports.Requests;
using CMMS.DAL.DTOs.Reports.Responses;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CMMS.BLL.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepo;
        private readonly IUserRepository _userRepo;
        private readonly IIotDataRepository _iotDataRepo;
        private readonly IReportEnvironmentSnapshotRepository _snapshotRepo;
        private readonly IReportAssignmentRepository _assignmentRepo;
        private readonly IDiagnosisResultRepository _diagnosisRepo;
        private readonly INotificationRepository _notificationRepo;
        private readonly IAttachmentService _attachmentService;
        private readonly IAttachmentRepository _attachmentRepo;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly INotificationRealtime _realtime;

        public ReportService(
            IReportRepository reportRepo,
            IUserRepository userRepo,
            IIotDataRepository iotDataRepo,
            IReportEnvironmentSnapshotRepository snapshotRepo,
            IReportAssignmentRepository assignmentRepo,
            IDiagnosisResultRepository diagnosisRepo,
            INotificationRepository notificationRepo,
            IAttachmentService attachmentService,
            IAttachmentRepository attachmentRepo,
            IServiceScopeFactory scopeFactory,
            INotificationRealtime realtime)
        {
            _reportRepo = reportRepo;
            _userRepo = userRepo;
            _iotDataRepo = iotDataRepo;
            _snapshotRepo = snapshotRepo;
            _assignmentRepo = assignmentRepo;
            _diagnosisRepo = diagnosisRepo;
            _notificationRepo = notificationRepo;
            _attachmentService = attachmentService;
            _attachmentRepo = attachmentRepo;
            _scopeFactory = scopeFactory;
            _realtime = realtime;
        }

        public async Task<ApiResponse<IEnumerable<ReportResponse>>> GetAllReportsAsync()
        {
            var reports = await _reportRepo.GetAllWithDetailsAsync();
            var data = new List<ReportResponse>();
            foreach (var r in reports)
            {
                var response = ReportMapper.ToResponse(r);
                var attachments = await _attachmentRepo.GetByObjectAsync("report", r.ReportId);
                response.Attachments = attachments.Select(AttachmentMapper.ToDto).ToList();
                data.Add(response);
            }
            return new ApiResponse<IEnumerable<ReportResponse>> { Success = true, Data = data };
        }

        public async Task<ApiResponse<IEnumerable<ReportResponse>>> FilterReportsAsync(string? status = null)
        {
            var reports = await _reportRepo.FilterAsync(status);
            var data = new List<ReportResponse>();
            foreach (var r in reports)
            {
                var response = ReportMapper.ToResponse(r);
                var attachments = await _attachmentRepo.GetByObjectAsync("report", r.ReportId);
                response.Attachments = attachments.Select(AttachmentMapper.ToDto).ToList();
                data.Add(response);
            }
            return new ApiResponse<IEnumerable<ReportResponse>> { Success = true, Data = data };
        }

        public async Task<ApiResponse<ReportResponse>> GetReportByIdAsync(Guid id)
        {
            var r = await _reportRepo.GetByIdWithDetailsAsync(id);
            if (r == null) return new ApiResponse<ReportResponse> { Success = false, Message = "Không tìm thấy báo cáo" };
            var response = ReportMapper.ToResponse(r);
            var attachments = await _attachmentRepo.GetByObjectAsync("report", r.ReportId);
            response.Attachments = attachments.Select(AttachmentMapper.ToDto).ToList();
            return new ApiResponse<ReportResponse> { Success = true, Data = response };
        }

        public async Task<ApiResponse<ReportResponse>> CreateReportAsync(CreateReportRequest request, Guid createdByUserId, List<IFormFile>? images = null)
        {
            var now = DateTimeHelper.VnNow();
            var reportNo = $"RPT-{now:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..3].ToUpper()}";

            var report = new Report
            {
                ReportId = Guid.NewGuid(),
                ReportNo = reportNo,
                WorkerId = createdByUserId,
                OwnerId = request.OwnerId,
                Title = request.Title,
                Description = request.Description,
                ReportType = request.ReportType,
                PlotId = request.PlotId,
                BedId = request.BedId,
                SeasonId = request.SeasonId,
                AiResultsJson = request.AiResultsJson,
                Status = "SENT_TO_OWNER",
                CreatedAt = now,
                SubmitDate = now
            };

            await _reportRepo.AddAsync(report);

            if (request.BedId.HasValue)
            {
                var latestData = await _iotDataRepo.GetLatestByBedIdAsync(request.BedId.Value, 1);
                var latest = latestData.FirstOrDefault();

                if (latest != null)
                {
                    var snapshot = new ReportEnvironmentSnapshot
                    {
                        Id = Guid.NewGuid(),
                        ReportId = report.ReportId,
                        Temperature = latest.Temperature,
                        Humidity = latest.Humidity,
                        SoilMoisture = latest.SoilMoisture,
                        Rainfall = latest.IsRaining.HasValue ? (latest.IsRaining.Value ? 1.0 : 0.0) : null,
                        LightIntensity = latest.Light,
                        RecordedAt = latest.RecordedAt ?? now,
                        SourceDeviceId = latest.DeviceId,
                        CreatedAt = now
                    };
                    await _snapshotRepo.AddAsync(snapshot);
                }
            }

            await _reportRepo.SaveChangesAsync();

            if (request.OwnerId.HasValue)
            {
                var reportId = report.ReportId;
                _ = System.Threading.Tasks.Task.Run(async () =>
                {
                    try
                    {
                        using var scope = _scopeFactory.CreateScope();
                        var notify = scope.ServiceProvider.GetRequiredService<INotificationService>();
                        await notify.NotifyNewReportAsync(reportId);
                    }
                    catch { }
                });
            }

            if (images != null && images.Any())
            {
                foreach (var img in images)
                {
                    await _attachmentService.UploadAsync(img, "report", report.ReportId, "report_image", createdByUserId);
                }
            }

            var created = await _reportRepo.GetByIdWithDetailsAsync(report.ReportId);
            var response = ReportMapper.ToResponse(created!);
            var createdAttachments = await _attachmentRepo.GetByObjectAsync("report", report.ReportId);
            response.Attachments = createdAttachments.Select(AttachmentMapper.ToDto).ToList();

            return new ApiResponse<ReportResponse> { Success = true, Data = response };
        }

        public async Task<ApiResponse<string>> AssignReportAsync(Guid reportId, AssignReportRequest request, Guid assignedByUserId)
        {
            var report = await _reportRepo.GetByIdAsync(reportId);
            if (report == null) return new ApiResponse<string> { Success = false, Message = "Không tìm thấy báo cáo" };

            var now = DateTimeHelper.VnNow();

            var assignment = new ReportAssignment
            {
                Id = Guid.NewGuid(),
                ReportId = reportId,
                AssignedBy = assignedByUserId,
                AssignedTo = request.AssignedTo,
                AssignedAt = now,
                Note = request.Note,
                Status = "ASSIGNED",
                CreatedAt = now
            };

            await _assignmentRepo.AddAsync(assignment);

            report.Status = "ASSIGNED_FOR_DIAGNOSIS";
            report.UpdatedAt = now;
            _reportRepo.Update(report);

            var notification = new Notification
            {
                NoteId = Guid.NewGuid(),
                UserId = request.AssignedTo,
                ReportId = reportId,
                NoteType = "report_assigned",
                NoteTitle = $"Bạn được phân công chẩn đoán báo cáo {report.ReportNo}",
                NoteMessage = request.Note,
                NoteStatus = "unread",
                NoteCreatedAt = now
            };
            await _notificationRepo.AddAsync(notification);

            await _reportRepo.SaveChangesAsync();

            await _realtime.PushToUserAsync(request.AssignedTo, new
            {
                noteId = notification.NoteId,
                noteType = notification.NoteType,
                noteTitle = notification.NoteTitle,
                noteMessage = notification.NoteMessage,
                reportId = reportId,
                createdAt = notification.NoteCreatedAt
            });

            return new ApiResponse<string> { Success = true, Message = "Phân công thành công" };
        }

        public async Task<ApiResponse<DiagnosisResponse>> CreateDiagnosisAsync(Guid reportId, CreateDiagnosisRequest request, Guid diagnosedByUserId)
        {
            var report = await _reportRepo.GetByIdAsync(reportId);
            if (report == null) return new ApiResponse<DiagnosisResponse> { Success = false, Message = "Không tìm thấy báo cáo" };

            var now = DateTimeHelper.VnNow();

            var diagnosis = new DiagnosisResult
            {
                DiagnosisResultId = Guid.NewGuid(),
                ReportId = reportId,
                DiagnosedBy = diagnosedByUserId,
                DiseaseName = request.DiseaseName,
                Conclusion = request.Conclusion,
                RecommendedAction = request.RecommendedAction,
                SeverityLevel = request.SeverityLevel,
                Status = "FINAL",
                CreatedAt = now
            };

            await _diagnosisRepo.AddAsync(diagnosis);

            report.Status = "DIAGNOSED";
            report.UpdatedAt = now;
            _reportRepo.Update(report);

            var latestAssignment = await _assignmentRepo.GetLatestByReportAndUserAsync(reportId, diagnosedByUserId);
            if (latestAssignment != null)
            {
                latestAssignment.Status = "DONE";
                latestAssignment.UpdatedAt = now;
            }

            Notification? ownerNotification = null;
            if (report.OwnerId.HasValue)
            {
                ownerNotification = new Notification
                {
                    NoteId = Guid.NewGuid(),
                    UserId = report.OwnerId.Value,
                    ReportId = reportId,
                    DiagnosisId = diagnosis.DiagnosisResultId,
                    NoteType = "diagnosis_completed",
                    NoteTitle = $"Chẩn đoán hoàn tất cho báo cáo {report.ReportNo}",
                    NoteMessage = $"Bệnh: {request.DiseaseName} - Mức độ: {request.SeverityLevel}",
                    NoteStatus = "unread",
                    NoteCreatedAt = now
                };
                await _notificationRepo.AddAsync(ownerNotification);
            }

            await _reportRepo.SaveChangesAsync();

            if (ownerNotification != null)
            {
                await _realtime.PushToUserAsync(ownerNotification.UserId!.Value, new
                {
                    noteId = ownerNotification.NoteId,
                    noteType = ownerNotification.NoteType,
                    noteTitle = ownerNotification.NoteTitle,
                    noteMessage = ownerNotification.NoteMessage,
                    reportId = reportId,
                    diagnosisId = ownerNotification.DiagnosisId,
                    createdAt = ownerNotification.NoteCreatedAt
                });
            }

            var diagnoser = await _userRepo.GetByIdAsync(diagnosedByUserId);

            return new ApiResponse<DiagnosisResponse>
            {
                Success = true,
                Data = ReportMapper.ToDiagnosisResponse(diagnosis, diagnoser?.Fullname)
            };
        }

        public async Task<ApiResponse<IEnumerable<DiagnosisResponse>>> GetAllDiagnosisAsync()
        {
            var list = await _diagnosisRepo.GetAllWithDetailsAsync();
            var data = list.Select(d => ReportMapper.ToDiagnosisResponse(d)).ToList();
            return new ApiResponse<IEnumerable<DiagnosisResponse>> { Success = true, Data = data };
        }

        public async Task<ApiResponse<DiagnosisResponse>> GetDiagnosisByIdAsync(Guid diagnosisId)
        {
            var d = await _diagnosisRepo.GetByIdWithDetailsAsync(diagnosisId);
            if (d == null) return new ApiResponse<DiagnosisResponse> { Success = false, Message = "Không tìm thấy kết quả chẩn đoán" };
            return new ApiResponse<DiagnosisResponse> { Success = true, Data = ReportMapper.ToDiagnosisResponse(d) };
        }

        public async Task<ApiResponse<IEnumerable<DiagnosisResponse>>> GetDiagnosisByReportIdAsync(Guid reportId)
        {
            var list = await _diagnosisRepo.GetByReportIdAsync(reportId);
            var data = list.Select(d => ReportMapper.ToDiagnosisResponse(d)).ToList();
            return new ApiResponse<IEnumerable<DiagnosisResponse>> { Success = true, Data = data };
        }

        public async Task<ApiResponse<string>> DeleteReportAsync(Guid id)
        {
            var report = await _reportRepo.GetByIdAsync(id);
            if (report == null) return new ApiResponse<string> { Success = false, Message = "Không tìm thấy" };

            _reportRepo.Delete(report);
            await _reportRepo.SaveChangesAsync();
            return new ApiResponse<string> { Success = true, Message = "Xóa thành công" };
        }

    }
}
