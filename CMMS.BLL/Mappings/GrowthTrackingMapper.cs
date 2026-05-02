using CMMS.DAL.DTOs.GrowthTrackings;
using CMMS.DAL.Entities;

namespace CMMS.BLL.Mappings
{
    public static class GrowthTrackingMapper
    {
        public static GrowthTrackingResponse ToResponse(GrowthTracking g) => new()
        {
            TrackingId = g.TrackingId,
            HarvestDetailId = g.HarvestDetailId,
            StageId = g.StageId,
            StageName = g.CropGrowthStage?.StageName,
            CropName = g.HarvestDetail?.Harvest?.Crop?.CropName,
            BedName = g.HarvestDetail?.Bed?.BedName,
            StartDate = g.StartDate,
            EndDate = g.EndDate,
            TrackingStatus = g.TrackingStatus,
            HealthStatus = g.HealthStatus,
            ActualHeight = g.ActualHeight,
            ActualYield = g.ActualYield,
            DelayDays = g.DelayDays,
            DelayReason = g.DelayReason,
            LastUpdatedBy = g.LastUpdatedBy,
            LastObservedAt = g.LastObservedAt,
            Notes = g.Notes,
            CreatedAt = g.CreatedAt,
            UpdatedAt = g.UpdatedAt
        };
    }
}
