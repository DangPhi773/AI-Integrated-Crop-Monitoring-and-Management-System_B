using System;
using System.Collections.Generic;

namespace CMMS.DAL.DTOs.GrowthTrackings
{
    public class GrowthTrackingRequest
    {
        public Guid HarvestDetailId { get; set; }
        public Guid StageId { get; set; }
        public DateTime? StartDate { get; set; }
        public string? HealthStatus { get; set; }
        public double? ActualHeight { get; set; }
        public string? Notes { get; set; }
    }

    public class GrowthTrackingUpdateRequest
    {
        public string? TrackingStatus { get; set; }
        public string? HealthStatus { get; set; }
        public double? ActualHeight { get; set; }
        public double? ActualYield { get; set; }
        public int? DelayDays { get; set; }
        public string? DelayReason { get; set; }
        public DateTime? LastObservedAt { get; set; }
        public string? Notes { get; set; }
    }

    public class AdvanceStageRequest
    {
        public Guid NextStageId { get; set; }
        public double? ActualHeight { get; set; }
        public double? ActualYield { get; set; }
        public string? Notes { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class GrowthTrackingResponse
    {
        public Guid TrackingId { get; set; }
        public Guid HarvestDetailId { get; set; }
        public Guid StageId { get; set; }
        public string? StageName { get; set; }
        public string? CropName { get; set; }
        public string? BedName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string TrackingStatus { get; set; } = "In-Progress";
        public string? HealthStatus { get; set; }
        public double? ActualHeight { get; set; }
        public double? ActualYield { get; set; }
        public int? DelayDays { get; set; }
        public string? DelayReason { get; set; }
        public Guid? LastUpdatedBy { get; set; }
        public DateTime? LastObservedAt { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class HarvestDetailProgressItem
    {
        public Guid HarvestDetailId { get; set; }
        public Guid? BedId { get; set; }
        public string? BedName { get; set; }
        public Guid? CropId { get; set; }
        public string? CropName { get; set; }
        public int TotalStages { get; set; }
        public int CompletedStages { get; set; }
        public int InProgressStages { get; set; }
        public double ProgressPercent { get; set; }
        public Guid? CurrentStageId { get; set; }
        public string? CurrentStageName { get; set; }
        public string? CurrentHealthStatus { get; set; }
        public int TotalDelayDays { get; set; }
        public DateTime? LastObservedAt { get; set; }
        public List<GrowthTrackingResponse> Trackings { get; set; } = new();
    }

    public class SeasonProgressResponse
    {
        public Guid SeasonId { get; set; }
        public string? SeasonName { get; set; }
        public string? Status { get; set; }
        public int TotalHarvestDetails { get; set; }
        public double OverallProgressPercent { get; set; }
        public List<HarvestDetailProgressItem> Items { get; set; } = new();
    }
}
