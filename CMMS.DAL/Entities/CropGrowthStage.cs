using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.Entities
{
    [Table("CropGrowthStage")]
    public class CropGrowthStage
    {
        [Key]
        public Guid StageId { get; set; }
        public Guid CropId { get; set; }

        [Required]
        [MaxLength(200)]
        public string StageName { get; set; }
        public string? StageDescription { get; set; }
        public double? TemperatureMax { get; set; }
        public double? HumidityMax { get; set; }
        public double? SoilMoistureMax { get; set; }
        public string? GrowthIndicators { get; set; }
        public string? CommonDiseases { get; set; }
        public string? Notes { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "timestamp")]
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("CropId")]
        public virtual Crop Crop { get; set; }
        public virtual ICollection<GrowthTracking> GrowthTrackings { get; set; } = new List<GrowthTracking>();
        public virtual ICollection<CropGrowthTask> CropGrowthTasks { get; set; } = new List<CropGrowthTask>();
    }
}
