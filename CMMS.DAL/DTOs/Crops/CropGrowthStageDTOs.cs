using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.DTOs.Crops
{
    public class CropGrowthStageRequest
    {
        public Guid CropId { get; set; }
        public string StageName { get; set; } = null!;
        public string? StageDescription { get; set; }
        public double? TemperatureMin { get; set; }
        public double? HumidityMin { get; set; }
        public double? SoilMoistureMin { get; set; }
        public string? GrowthIndicators { get; set; }
        public string? CommonDiseases { get; set; }
        public string? Notes { get; set; }
    }

    public class CropGrowthStageResponse
    {
        public Guid StageId { get; set; }
        public Guid CropId { get; set; }
        public string? CropName { get; set; }
        public string StageName { get; set; } = null!;
        public string? StageDescription { get; set; }
        public double? TemperatureMin { get; set; }
        public double? HumidityMin { get; set; }
        public double? SoilMoistureMin { get; set; }
        public string? GrowthIndicators { get; set; }
        public string? CommonDiseases { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
