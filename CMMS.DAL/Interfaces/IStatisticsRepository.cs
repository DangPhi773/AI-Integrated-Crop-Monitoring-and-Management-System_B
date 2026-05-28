using CMMS.DAL.DTOs.Statistics.Request;
using CMMS.DAL.DTOs.Statistics.Response;

namespace CMMS.DAL.Interfaces
{
    public interface IStatisticsRepository
    {
        Task<IEnumerable<YieldByCropResponse>> GetYieldByCropAsync(YieldStatsFilter filter);
        Task<IEnumerable<YieldBySeasonResponse>> GetYieldBySeasonAsync(YieldStatsFilter filter);
        Task<IEnumerable<YieldByPlotResponse>> GetYieldByPlotAsync(YieldStatsFilter filter);
        Task<YieldSummaryResponse> GetYieldSummaryAsync(YieldStatsFilter filter);
    }
}
