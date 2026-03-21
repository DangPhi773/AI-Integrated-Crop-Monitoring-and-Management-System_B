using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Interfaces
{
    public interface IReportService
    {
        Task<ApiResponse<IEnumerable<ReportResponse>>> GetAllReportsAsync();
        Task<ApiResponse<ReportResponse>> GetReportByIdAsync(Guid id);
        Task<ApiResponse<string>> CreateReportAsync(ReportRequest request);
        Task<ApiResponse<string>> UpdateReportAsync(Guid id, ReportRequest request);
        Task<ApiResponse<string>> DeleteReportAsync(Guid id);
    }
}
