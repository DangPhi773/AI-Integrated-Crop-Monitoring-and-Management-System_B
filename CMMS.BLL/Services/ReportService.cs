using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Reports;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepo;

        public ReportService(IReportRepository reportRepo)
        {
            _reportRepo = reportRepo;
        }

        public async Task<ApiResponse<IEnumerable<ReportResponse>>> GetAllReportsAsync()
        {
            try
            {
                var reports = await _reportRepo.GetAllWithWorkerAsync();
                var response = reports.Select(r => new ReportResponse
                {
                    ReportId = r.ReportId,
                    WorkerId = r.WorkerId,
                    WorkerName = r.Worker?.Fullname,
                    Title = r.Title,
                    Description = r.Description,
                    Status = r.Status,
                    CreatedAt = r.CreatedAt,
                    SubmitDate = r.SubmitDate
                }).ToList();

                return new ApiResponse<IEnumerable<ReportResponse>> { Success = true, Data = response };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<ReportResponse>> { Success = false, Message = "Lỗi hệ thống", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<ReportResponse>> GetReportByIdAsync(Guid id)
        {
            try
            {
                var r = await _reportRepo.GetByIdWithWorkerAsync(id);
                if (r == null)
                    return new ApiResponse<ReportResponse> { Success = false, Message = "Không tìm thấy báo cáo" };

                var response = new ReportResponse
                {
                    ReportId = r.ReportId,
                    WorkerId = r.WorkerId,
                    WorkerName = r.Worker?.Fullname,
                    Title = r.Title,
                    Description = r.Description,
                    Status = r.Status,
                    CreatedAt = r.CreatedAt,
                    SubmitDate = r.SubmitDate
                };

                return new ApiResponse<ReportResponse> { Success = true, Data = response };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ReportResponse> { Success = false, Message = "Lỗi", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> CreateReportAsync(ReportRequest request)
        {
            try
            {
                var report = new Report
                {
                    ReportId = Guid.NewGuid(),
                    WorkerId = request.WorkerId,
                    Title = request.Title,
                    Description = request.Description,
                    Status = request.Status ?? "Pending",
                    CreatedAt = DateTime.UtcNow,
                    SubmitDate = request.SubmitDate
                };

                await _reportRepo.AddAsync(report);
                var result = await _reportRepo.SaveChangesAsync();

                return result
                    ? new ApiResponse<string> { Success = true, Message = "Tạo báo cáo thành công" }
                    : new ApiResponse<string> { Success = false, Message = "Lưu thất bại" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Lỗi", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> UpdateReportAsync(Guid id, ReportRequest request)
        {
            try
            {
                var report = await _reportRepo.GetByIdAsync(id);
                if (report == null) return new ApiResponse<string> { Success = false, Message = "Không tìm thấy" };

                report.Title = request.Title;
                report.Description = request.Description;
                report.Status = request.Status;
                report.WorkerId = request.WorkerId;
                report.SubmitDate = request.SubmitDate;

                _reportRepo.Update(report);
                await _reportRepo.SaveChangesAsync();

                return new ApiResponse<string> { Success = true, Message = "Cập nhật thành công" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> DeleteReportAsync(Guid id)
        {
            try
            {
                var report = await _reportRepo.GetByIdAsync(id);
                if (report == null) return new ApiResponse<string> { Success = false, Message = "Không tìm thấy" };

                _reportRepo.Delete(report);
                await _reportRepo.SaveChangesAsync();

                return new ApiResponse<string> { Success = true, Message = "Xóa thành công" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Errors = new List<string> { ex.Message } };
            }
        }
    }
}
