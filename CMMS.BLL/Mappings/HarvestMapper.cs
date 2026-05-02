using CMMS.DAL.DTOs.Harvests;
using CMMS.DAL.Entities;

namespace CMMS.BLL.Mappings
{
    public static class HarvestMapper
    {
        public static HarvestResponse ToResponse(Harvest h)
        {
            var totalHarvested = h.HarvestRecords?.Sum(r => r.Quantity) ?? 0m;
            var totalSold = h.HarvestRecords?.Where(r => r.SoldQuantity.HasValue).Sum(r => r.SoldQuantity!.Value) ?? 0m;
            var totalRevenue = h.HarvestRecords?.Where(r => r.TotalAmount.HasValue).Sum(r => r.TotalAmount!.Value) ?? 0m;

            return new HarvestResponse
            {
                HarvestId = h.HarvestId,
                PlotId = h.PlotId,
                PlotName = h.Plot?.PlotName,
                SeasonId = h.SeasonId,
                SeasonName = h.Season?.SeasonName,
                CropId = h.CropId,
                CropName = h.Crop?.CropName,
                ExpectedDate = h.ExpectedDate,
                ExpectedQuantity = h.ExpectedQuantity,
                Unit = h.Unit,
                Status = h.Status,
                Notes = h.Notes,
                CreatedAt = h.CreatedAt,
                UpdatedAt = h.UpdatedAt,
                HarvestDetails = h.HarvestDetails?.Select(d => new HarvestDetailDto
                {
                    HarvestDetailId = d.HarvestDetailId,
                    BedId = d.BedId,
                    BedName = d.Bed?.BedName,
                    CropQuantity = d.CropQuantity,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate
                }).ToList() ?? new List<HarvestDetailDto>(),
                RecordsCount = h.HarvestRecords?.Count ?? 0,
                TotalHarvestedQuantity = totalHarvested,
                TotalSoldQuantity = totalSold,
                TotalRevenue = totalRevenue
            };
        }

        public static HarvestSummary ToSummary(Harvest h) => new()
        {
            HarvestId = h.HarvestId,
            PlotId = h.PlotId,
            PlotName = h.Plot?.PlotName,
            CropId = h.CropId,
            CropName = h.Crop?.CropName,
            SeasonId = h.SeasonId,
            SeasonName = h.Season?.SeasonName,
            ExpectedDate = h.ExpectedDate,
            ExpectedQuantity = h.ExpectedQuantity,
            Unit = h.Unit,
            Status = h.Status,
            DetailsCount = h.HarvestDetails?.Count ?? 0,
            RecordsCount = h.HarvestRecords?.Count ?? 0
        };
    }
}
