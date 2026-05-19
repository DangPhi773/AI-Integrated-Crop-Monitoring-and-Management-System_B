using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMMS.DAL.DTOs.GrowthTrackings
{
    public class GrowthTrackingRequest
    {
        [Required(ErrorMessage = "HarvestDetailId là bắt buộc")]
        public Guid HarvestDetailId { get; set; }

        [Required(ErrorMessage = "StageId là bắt buộc")]
        public Guid StageId { get; set; }

        public DateTime? StartDate { get; set; }

        [StringLength(50, ErrorMessage = "HealthStatus tối đa 50 ký tự")]
        public string? HealthStatus { get; set; }

        [Range(0.0, 1000, ErrorMessage = "ActualHeight phải từ 0 đến 1000")]
        public double? ActualHeight { get; set; }

        [StringLength(1000, ErrorMessage = "Notes tối đa 1000 ký tự")]
        public string? Notes { get; set; }
    }

    public class GrowthTrackingUpdateRequest
    {
        [StringLength(50, ErrorMessage = "TrackingStatus tối đa 50 ký tự")]
        public string? TrackingStatus { get; set; }

        [StringLength(50, ErrorMessage = "HealthStatus tối đa 50 ký tự")]
        public string? HealthStatus { get; set; }

        [Range(0.0, 1000, ErrorMessage = "ActualHeight phải từ 0 đến 1000")]
        public double? ActualHeight { get; set; }

        [Range(0.0, 1000000, ErrorMessage = "ActualYield phải từ 0 đến 1,000,000")]
        public double? ActualYield { get; set; }

        [Range(0, 365, ErrorMessage = "DelayDays phải từ 0 đến 365")]
        public int? DelayDays { get; set; }

        [StringLength(500, ErrorMessage = "DelayReason tối đa 500 ký tự")]
        public string? DelayReason { get; set; }

        public DateTime? LastObservedAt { get; set; }

        [StringLength(1000, ErrorMessage = "Notes tối đa 1000 ký tự")]
        public string? Notes { get; set; }
    }

    public class AdvanceStageRequest
    {
        [Required(ErrorMessage = "NextStageId là bắt buộc")]
        public Guid NextStageId { get; set; }

        [Range(0.0, 1000, ErrorMessage = "ActualHeight phải từ 0 đến 1000")]
        public double? ActualHeight { get; set; }

        [Range(0.0, 1000000, ErrorMessage = "ActualYield phải từ 0 đến 1,000,000")]
        public double? ActualYield { get; set; }

        [StringLength(1000, ErrorMessage = "Notes tối đa 1000 ký tự")]
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
