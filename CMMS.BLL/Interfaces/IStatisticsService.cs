using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Statistics.Request;
using CMMS.DAL.DTOs.Statistics.Response;

namespace CMMS.BLL.Interfaces
{
    public interface IStatisticsService
    {
        Task<ApiResponse<IEnumerable<YieldByCropResponse>>> GetYieldByCropAsync(YieldStatsFilter filter);
        Task<ApiResponse<IEnumerable<YieldBySeasonResponse>>> GetYieldBySeasonAsync(YieldStatsFilter filter);
        Task<ApiResponse<IEnumerable<YieldByPlotResponse>>> GetYieldByPlotAsync(YieldStatsFilter filter);
        Task<ApiResponse<YieldSummaryResponse>> GetYieldSummaryAsync(YieldStatsFilter filter);
    }
}
