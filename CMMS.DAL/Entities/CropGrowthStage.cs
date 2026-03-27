using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.Entities
{
    public class CropGrowthStage
    {
            public Guid StageId { get; set; }
            public Guid CropId { get; set; } 
            public string StageName { get; set; }
            public string? StageDescription { get; set; }
            public double? TemperatureMin { get; set; }
            public double? HumidityMin { get; set; }
            public double? SoilMoistureMin { get; set; }
            public string? GrowthIndicators { get; set; }
            public string? CommonDiseases { get; set; }
            public string? Notes { get; set; }
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
            public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

            public virtual Crop Crop { get; set; }
            public virtual ICollection<GrowthTracking> GrowthTrackings { get; set; } = new List<GrowthTracking>();
            public virtual ICollection<CropGrowthTask> CropGrowthTasks { get; set; } = new List<CropGrowthTask>();
    }
}
