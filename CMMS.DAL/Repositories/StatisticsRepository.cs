using CMMS.DAL.DBContext;
using CMMS.DAL.DTOs.Statistics.Request;
using CMMS.DAL.DTOs.Statistics.Response;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMMS.DAL.Repositories
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly AppDbContext _context;
        public StatisticsRepository(AppDbContext context) => _context = context;

        private IQueryable<HarvestDetail> BuildBaseQuery(YieldStatsFilter f)
        {
            var q = _context.HarvestDetails
                .Include(d => d.Harvest).ThenInclude(h => h.Crop)
                .Include(d => d.Harvest).ThenInclude(h => h.Season)
                .Include(d => d.Harvest).ThenInclude(h => h.Plot)
                .AsNoTracking()
                .AsQueryable();

            if (f.From.HasValue)
                q = q.Where(d => d.ActualHarvestDate != null && d.ActualHarvestDate >= f.From);
            if (f.To.HasValue)
                q = q.Where(d => d.ActualHarvestDate != null && d.ActualHarvestDate <= f.To);
            if (f.CropId.HasValue)
                q = q.Where(d => d.Harvest.CropId == f.CropId);
            if (f.SeasonId.HasValue)
                q = q.Where(d => d.Harvest.SeasonId == f.SeasonId);
            if (f.FarmId.HasValue)
                q = q.Where(d => d.Harvest.Plot.FarmId == f.FarmId);

            return q;
        }

        public async Task<IEnumerable<YieldByCropResponse>> GetYieldByCropAsync(YieldStatsFilter f)
        {
            return await BuildBaseQuery(f)
                .GroupBy(d => new { d.Harvest.CropId, d.Harvest.Crop.CropName })
                .Select(g => new YieldByCropResponse
                {
                    CropId = g.Key.CropId,
                    CropName = g.Key.CropName ?? string.Empty,
                    HarvestCount = g.Select(x => x.HarvestId).Distinct().Count(),
                    CompletedDetailsCount = g.Count(x => x.ActualHarvestDate != null),
                    TotalActualWeightKg = g.Sum(x => x.ActualWeightKg ?? 0m),
                    TotalActualQuantity = g.Sum(x => x.ActualQuantity ?? 0),
                    TotalExpectedQuantity = g.Sum(x => x.Harvest.ExpectedQuantity ?? 0m),
                    SeasonsCovered = g.Select(x => x.Harvest.SeasonId).Distinct().Count()
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<YieldBySeasonResponse>> GetYieldBySeasonAsync(YieldStatsFilter f)
        {
            return await BuildBaseQuery(f)
                .GroupBy(d => new
                {
                    d.Harvest.SeasonId,
                    d.Harvest.Season.SeasonName,
                    d.Harvest.Season.SeasonStartDate,
                    d.Harvest.Season.SeasonEndDate
                })
                .Select(g => new YieldBySeasonResponse
                {
                    SeasonId = g.Key.SeasonId,
                    SeasonName = g.Key.SeasonName,
                    SeasonStartDate = g.Key.SeasonStartDate,
                    SeasonEndDate = g.Key.SeasonEndDate,
                    HarvestCount = g.Select(x => x.HarvestId).Distinct().Count(),
                    TotalActualWeightKg = g.Sum(x => x.ActualWeightKg ?? 0m),
                    TotalActualQuantity = g.Sum(x => x.ActualQuantity ?? 0),
                    TotalExpectedQuantity = g.Sum(x => x.Harvest.ExpectedQuantity ?? 0m),
                    CropsCovered = g.Select(x => x.Harvest.CropId).Distinct().Count(),
                    PlotsCovered = g.Select(x => x.Harvest.PlotId).Distinct().Count()
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<YieldByPlotResponse>> GetYieldByPlotAsync(YieldStatsFilter f)
        {
            return await BuildBaseQuery(f)
                .GroupBy(d => new { d.Harvest.PlotId, d.Harvest.Plot.PlotName, d.Harvest.Plot.PlotArea })
                .Select(g => new YieldByPlotResponse
                {
                    PlotId = g.Key.PlotId,
                    PlotName = g.Key.PlotName,
                    PlotArea = g.Key.PlotArea,
                    HarvestCount = g.Select(x => x.HarvestId).Distinct().Count(),
                    TotalActualWeightKg = g.Sum(x => x.ActualWeightKg ?? 0m),
                    CropsCovered = g.Select(x => x.Harvest.CropId).Distinct().Count()
                })
                .ToListAsync();
        }

        public async Task<YieldSummaryResponse> GetYieldSummaryAsync(YieldStatsFilter f)
        {
            var baseQuery = BuildBaseQuery(f);

            var harvestsQuery = _context.Harvests.AsNoTracking().AsQueryable();
            if (f.FarmId.HasValue)
                harvestsQuery = harvestsQuery.Where(h => h.Plot.FarmId == f.FarmId);
            if (f.From.HasValue)
                harvestsQuery = harvestsQuery.Where(h => h.ExpectedDate == null || h.ExpectedDate >= f.From);
            if (f.To.HasValue)
                harvestsQuery = harvestsQuery.Where(h => h.ExpectedDate == null || h.ExpectedDate <= f.To);

            var totalHarvests = await harvestsQuery.CountAsync();
            var completedHarvests = await harvestsQuery.CountAsync(h => h.Status == "completed");
            var ongoingHarvests = await harvestsQuery
                .CountAsync(h => h.Status == "growing" || h.Status == "harvesting");

            var aggregates = await baseQuery
                .GroupBy(_ => 1)
                .Select(g => new
                {
                    TotalActualWeightKg = g.Sum(x => x.ActualWeightKg ?? 0m),
                    TotalExpectedQuantity = g.Sum(x => x.Harvest.ExpectedQuantity ?? 0m),
                    CropsCount = g.Select(x => x.Harvest.CropId).Distinct().Count(),
                    SeasonsCount = g.Select(x => x.Harvest.SeasonId).Distinct().Count(),
                    PlotsCount = g.Select(x => x.Harvest.PlotId).Distinct().Count()
                })
                .FirstOrDefaultAsync();

            var topCrop = await baseQuery
                .GroupBy(d => new { d.Harvest.CropId, d.Harvest.Crop.CropName })
                .Select(g => new
                {
                    g.Key.CropId,
                    g.Key.CropName,
                    Total = g.Sum(x => x.ActualWeightKg ?? 0m)
                })
                .OrderByDescending(x => x.Total)
                .FirstOrDefaultAsync();

            return new YieldSummaryResponse
            {
                From = f.From,
                To = f.To,
                TotalHarvests = totalHarvests,
                CompletedHarvests = completedHarvests,
                OngoingHarvests = ongoingHarvests,
                TotalActualWeightKg = aggregates?.TotalActualWeightKg ?? 0m,
                TotalExpectedQuantity = aggregates?.TotalExpectedQuantity ?? 0m,
                CropsCount = aggregates?.CropsCount ?? 0,
                SeasonsCount = aggregates?.SeasonsCount ?? 0,
                PlotsCount = aggregates?.PlotsCount ?? 0,
                TopCropId = topCrop?.CropId,
                TopCropName = topCrop?.CropName,
                TopCropWeightKg = topCrop?.Total ?? 0m
            };
        }
    }
}
