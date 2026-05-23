using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities
{
    [Table("GrowthTracking")]
    public class GrowthTracking
    {
        [Key]
        public Guid TrackingId { get; set; }

        [Required]
        public Guid HarvestDetailId { get; set; }

        [Required]
        public Guid StageId { get; set; }

        [Required]
        [Column(TypeName = "timestamp")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? EndDate { get; set; }

        [StringLength(50)]
        public string TrackingStatus { get; set; } = "In-Progress";

        [StringLength(100)]
        public string? HealthStatus { get; set; }

        public double? ActualHeight { get; set; }

        public double? ActualYield { get; set; }

        public int? DelayDays { get; set; }

        public string? DelayReason { get; set; }

        public Guid? LastUpdatedBy { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? LastObservedAt { get; set; }

        public string? Notes { get; set; }

        [Required]
        [Column(TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [Column(TypeName = "timestamp")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("HarvestDetailId")]
        public virtual HarvestDetail HarvestDetail { get; set; } = null!;

        [ForeignKey("StageId")]
        public virtual CropGrowthStage CropGrowthStage { get; set; } = null!;
    }
}
