using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Statistics.Request;
using CMMS.DAL.DTOs.Statistics.Response;
using CMMS.DAL.Interfaces;

namespace CMMS.BLL.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IStatisticsRepository _repo;
        public StatisticsService(IStatisticsRepository repo) => _repo = repo;

        public async Task<ApiResponse<IEnumerable<YieldByCropResponse>>> GetYieldByCropAsync(YieldStatsFilter filter)
        {
            try
            {
                var raw = await _repo.GetYieldByCropAsync(filter);
                var data = raw
                    .Select(r =>
                    {
                        r.AvgWeightKgPerHarvest = r.HarvestCount > 0
                            ? Math.Round(r.TotalActualWeightKg / r.HarvestCount, 2)
                            : 0m;
                        r.FulfillmentRate = r.TotalExpectedQuantity > 0
                            ? Math.Round(r.TotalActualWeightKg / r.TotalExpectedQuantity * 100m, 2)
                            : (decimal?)null;
                        return r;
                    })
                    .OrderByDescending(r => r.TotalActualWeightKg)
                    .ToList();

                return new ApiResponse<IEnumerable<YieldByCropResponse>> { Success = true, Data = data };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<YieldByCropResponse>>
                {
                    Success = false,
                    Message = "Lỗi lấy thống kê năng suất theo crop",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<YieldBySeasonResponse>>> GetYieldBySeasonAsync(YieldStatsFilter filter)
        {
            try
            {
                var raw = await _repo.GetYieldBySeasonAsync(filter);
                var data = raw
                    .Select(r =>
                    {
                        r.FulfillmentRate = r.TotalExpectedQuantity > 0
                            ? Math.Round(r.TotalActualWeightKg / r.TotalExpectedQuantity * 100m, 2)
                            : (decimal?)null;
                        return r;
                    })
                    .OrderByDescending(r => r.SeasonStartDate)
                    .ToList();

                return new ApiResponse<IEnumerable<YieldBySeasonResponse>> { Success = true, Data = data };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<YieldBySeasonResponse>>
                {
                    Success = false,
                    Message = "Lỗi lấy thống kê năng suất theo mùa vụ",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<YieldByPlotResponse>>> GetYieldByPlotAsync(YieldStatsFilter filter)
        {
            try
            {
                var raw = await _repo.GetYieldByPlotAsync(filter);
                var data = raw
                    .Select(r =>
                    {
                        r.YieldPerAreaKg = r.PlotArea.HasValue && r.PlotArea.Value > 0
                            ? Math.Round(r.TotalActualWeightKg / r.PlotArea.Value, 2)
                            : (decimal?)null;
                        return r;
                    })
                    .OrderByDescending(r => r.TotalActualWeightKg)
                    .ToList();

                return new ApiResponse<IEnumerable<YieldByPlotResponse>> { Success = true, Data = data };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<YieldByPlotResponse>>
                {
                    Success = false,
                    Message = "Lỗi lấy thống kê năng suất theo lô đất",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<YieldSummaryResponse>> GetYieldSummaryAsync(YieldStatsFilter filter)
        {
            try
            {
                var data = await _repo.GetYieldSummaryAsync(filter);
                data.OverallFulfillmentRate = data.TotalExpectedQuantity > 0
                    ? Math.Round(data.TotalActualWeightKg / data.TotalExpectedQuantity * 100m, 2)
                    : (decimal?)null;

                return new ApiResponse<YieldSummaryResponse> { Success = true, Data = data };
            }
            catch (Exception ex)
            {
                return new ApiResponse<YieldSummaryResponse>
                {
                    Success = false,
                    Message = "Lỗi lấy tổng quan năng suất",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
