using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.Entities
{
    public class GrowthTracking
    {
        public Guid TrackingId { get; set; }
        public Guid SeasonDetailId { get; set; } 
        public Guid StageId { get; set; }
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
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public virtual SeasonsDetail SeasonDetail { get; set; }
        public virtual CropGrowthStage CropGrowthStage { get; set; }
    }
}
