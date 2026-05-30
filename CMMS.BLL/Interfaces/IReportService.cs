using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Reports.Requests;
using CMMS.DAL.DTOs.Reports.Responses;
using Microsoft.AspNetCore.Http;

namespace CMMS.BLL.Interfaces
{
    public interface IReportService
    {
        Task<ApiResponse<IEnumerable<ReportResponse>>> GetAllReportsAsync();
        Task<ApiResponse<IEnumerable<ReportResponse>>> FilterReportsAsync(string? status = null);
        Task<ApiResponse<ReportResponse>> GetReportByIdAsync(Guid id);
        Task<ApiResponse<ReportResponse>> CreateReportAsync(CreateReportRequest request, Guid createdByUserId, List<IFormFile>? images = null);
        Task<ApiResponse<string>> AssignReportAsync(Guid reportId, AssignReportRequest request, Guid assignedByUserId);
        Task<ApiResponse<DiagnosisResponse>> CreateDiagnosisAsync(Guid reportId, CreateDiagnosisRequest request, Guid diagnosedByUserId);
        Task<ApiResponse<IEnumerable<DiagnosisResponse>>> GetAllDiagnosisAsync(Guid userId, string role);
        Task<ApiResponse<DiagnosisResponse>> GetDiagnosisByIdAsync(Guid diagnosisId, Guid userId, string role);
        Task<ApiResponse<IEnumerable<DiagnosisResponse>>> GetDiagnosisByReportIdAsync(Guid reportId, Guid userId, string role);
        Task<ApiResponse<string>> DeleteReportAsync(Guid id);
    }
}
