using CMMS.DAL.DTOs.Harvests;
using CMMS.DAL.Entities;

namespace CMMS.BLL.Mappings
{
    public static class HarvestMapper
    {
        public static HarvestResponse ToResponse(Harvest h)
        {
            var details = h.HarvestDetails ?? new List<HarvestDetail>();
            var hasAnyQuantity = details.Any(d => d.ActualQuantity.HasValue);
            var hasAnyWeight = details.Any(d => d.ActualWeightKg.HasValue);
            var totalQuantity = details.Where(d => d.ActualQuantity.HasValue).Sum(d => d.ActualQuantity!.Value);
            var totalWeightKg = details.Where(d => d.ActualWeightKg.HasValue).Sum(d => d.ActualWeightKg!.Value);
            var harvestedBedsCount = details.Count(d => d.ActualHarvestDate.HasValue);

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
                HarvestDetails = details.Select(d => new HarvestDetailDto
                {
                    HarvestDetailId = d.HarvestDetailId,
                    BedId = d.BedId,
                    BedName = d.Bed?.BedName,
                    CropQuantity = d.CropQuantity,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate,
                    ActualHarvestDate = d.ActualHarvestDate,
                    ActualQuantity = d.ActualQuantity,
                    ActualWeightKg = d.ActualWeightKg,
                    HarvestNotes = d.HarvestNotes
                }).ToList(),
                HarvestedBedsCount = harvestedBedsCount,
                TotalHarvestedQuantity = hasAnyQuantity ? totalQuantity : null,
                TotalHarvestedWeightKg = hasAnyWeight ? totalWeightKg : null
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
            HarvestedBedsCount = h.HarvestDetails?.Count(d => d.ActualHarvestDate.HasValue) ?? 0
        };
    }
}
