using CMMS.DAL.DTOs.Harvests;
using CMMS.DAL.Entities;

namespace CMMS.BLL.Mappings
{
    public static class HarvestDetailMapper
    {
        public static HarvestDetailResponse ToResponse(HarvestDetail d) => new()
        {
            HarvestDetailId = d.HarvestDetailId,
            HarvestId = d.HarvestId,
            BedId = d.BedId,
            BedName = d.Bed?.BedName,
            CropId = d.Harvest?.CropId,
            CropName = d.Harvest?.Crop?.CropName,
            PlotId = d.Harvest?.PlotId,
            PlotName = d.Harvest?.Plot?.PlotName,
            SeasonId = d.Harvest?.SeasonId,
            SeasonName = d.Harvest?.Season?.SeasonName,
            CropQuantity = d.CropQuantity,
            StartDate = d.StartDate,
            EndDate = d.EndDate,
            ActualHarvestDate = d.ActualHarvestDate,
            ActualQuantity = d.ActualQuantity,
            ActualWeightKg = d.ActualWeightKg,
            HarvestNotes = d.HarvestNotes
        };
    }
}
