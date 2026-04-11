using CMMS.BLL.Helpers;
using CMMS.BLL.Interfaces;
using CMMS.BLL.Mappings;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Reports.Requests;
using CMMS.DAL.DTOs.Reports.Responses;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
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
        private readonly IServiceScopeFactory _scopeFactory;

        public ReportService(
            IReportRepository reportRepo,
            IUserRepository userRepo,
            IIotDataRepository iotDataRepo,
            IReportEnvironmentSnapshotRepository snapshotRepo,
            IReportAssignmentRepository assignmentRepo,
            IDiagnosisResultRepository diagnosisRepo,
            INotificationRepository notificationRepo,
            IServiceScopeFactory scopeFactory)
        {
            _reportRepo = reportRepo;
            _userRepo = userRepo;
            _iotDataRepo = iotDataRepo;
            _snapshotRepo = snapshotRepo;
            _assignmentRepo = assignmentRepo;
            _diagnosisRepo = diagnosisRepo;
            _notificationRepo = notificationRepo;
            _scopeFactory = scopeFactory;
        }

        public async Task<ApiResponse<IEnumerable<ReportResponse>>> GetAllReportsAsync()
        {
            var reports = await _reportRepo.GetAllWithDetailsAsync();
            var data = reports.Select(ReportMapper.ToResponse).ToList();
            return new ApiResponse<IEnumerable<ReportResponse>> { Success = true, Data = data };
        }

        public async Task<ApiResponse<ReportResponse>> GetReportByIdAsync(Guid id)
        {
            var r = await _reportRepo.GetByIdWithDetailsAsync(id);
            if (r == null) return new ApiResponse<ReportResponse> { Success = false, Message = "Không tìm thấy báo cáo" };
            return new ApiResponse<ReportResponse> { Success = true, Data = ReportMapper.ToResponse(r) };
        }

        public async Task<ApiResponse<ReportResponse>> CreateReportAsync(CreateReportRequest request, Guid createdByUserId)
        {
            var now = DateTimeHelper.VnNow();
            var reportNo = $"RPT-{now:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..3].ToUpper()}";

            var report = new Report
            {
                ReportId = Guid.NewGuid(),
                ReportNo = reportNo,
                CreatedBy = createdByUserId,
                OwnerId = request.OwnerId,
                Title = request.Title,
                Description = request.Description,
                ReportType = request.ReportType,
                PlotId = request.PlotId,
                BedId = request.BedId,
                SeasonId = request.SeasonId,
                //AiResultsJson = request.AiResultsJson,
                Status = "SENT_TO_OWNER",
                CreatedAt = now,
                SubmitDate = now
            };

            await _reportRepo.AddAsync(report);

            if (request.BedId.HasValue)
            {
                var latestData = await _iotDataRepo.GetLatestByBedIdAsync(request.BedId.Value, 10);

                if (latestData.Any())
                {
                    var snapshot = new ReportEnvironmentSnapshot
                    {
                        Id = Guid.NewGuid(),
                        ReportId = report.ReportId,
                        Temperature = latestData.FirstOrDefault(d => d.Type == "TEMPERATURE")?.Value,
                        Humidity = latestData.FirstOrDefault(d => d.Type == "HUMIDITY")?.Value,
                        SoilMoisture = latestData.FirstOrDefault(d => d.Type == "SOIL_MOISTURE")?.Value,
                        Rainfall = latestData.FirstOrDefault(d => d.Type == "RAIN")?.Value,
                        LightIntensity = latestData.FirstOrDefault(d => d.Type == "LIGHT")?.Value,
                        RecordedAt = latestData.Max(d => d.RecordedAt) ?? now,
                        SourceDeviceId = latestData.First().DeviceId,
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

            var created = await _reportRepo.GetByIdWithDetailsAsync(report.ReportId);

            return new ApiResponse<ReportResponse> { Success = true, Data = ReportMapper.ToResponse(created!) };
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

            await _notificationRepo.AddAsync(new Notification
            {
                NoteId = Guid.NewGuid(),
                UserId = request.AssignedTo,
                ReportId = reportId,
                NoteType = "report_assigned",
                NoteTitle = $"Bạn được phân công chẩn đoán báo cáo {report.ReportNo}",
                NoteMessage = request.Note,
                NoteStatus = "unread",
                NoteCreatedAt = now
            });

            await _reportRepo.SaveChangesAsync();
            return new ApiResponse<string> { Success = true, Message = "Phân công thành công" };
        }

        public async Task<ApiResponse<DiagnosisResponse>> CreateDiagnosisAsync(Guid reportId, CreateDiagnosisRequest request, Guid diagnosedByUserId)
        {
            var report = await _reportRepo.GetByIdAsync(reportId);
            if (report == null) return new ApiResponse<DiagnosisResponse> { Success = false, Message = "Không tìm thấy báo cáo" };

            var now = DateTimeHelper.VnNow();

            var diagnosis = new DiagnosisResult
            {
                Id = Guid.NewGuid(),
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

            if (report.OwnerId.HasValue)
            {
                await _notificationRepo.AddAsync(new Notification
                {
                    NoteId = Guid.NewGuid(),
                    UserId = report.OwnerId.Value,
                    ReportId = reportId,
                    DiagnosisId = diagnosis.Id,
                    NoteType = "diagnosis_completed",
                    NoteTitle = $"Chẩn đoán hoàn tất cho báo cáo {report.ReportNo}",
                    NoteMessage = $"Bệnh: {request.DiseaseName} - Mức độ: {request.SeverityLevel}",
                    NoteStatus = "unread",
                    NoteCreatedAt = now
                });
            }

            await _reportRepo.SaveChangesAsync();

            var diagnoser = await _userRepo.GetByIdAsync(diagnosedByUserId);

            return new ApiResponse<DiagnosisResponse>
            {
                Success = true,
                Data = ReportMapper.ToResponse(diagnosis, diagnoser?.Fullname)
            };
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
