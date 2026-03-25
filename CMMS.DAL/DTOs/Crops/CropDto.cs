using System;
using System.Collections.Generic;

namespace CMMS.DAL.DTOs.Crops
{
    public class CropRequest
    {
        public string CropName { get; set; } = null!;
        public string? CropScientificName { get; set; }
        public int? CropDefaultGrowthDays { get; set; }
        public double? PlantSpacing { get; set; }
        public int? CropQuantities { get; set; }
        public string? CropStatus { get; set; }
    }

    public class CropResponse
    {
        public Guid CropId { get; set; }
        public string CropName { get; set; } = null!;
        public string? CropScientificName { get; set; }
        public int? CropDefaultGrowthDays { get; set; }
        public double? PlantSpacing { get; set; }
        public int? CropQuantities { get; set; }
        public string? CropStatus { get; set; }
        public List<SoilCompatibilityDto> CompatibleSoils { get; set; } = new List<SoilCompatibilityDto>();
    }

    public class SoilCompatibilityDto
    {
        public Guid SoilId { get; set; }
        public string? SoilName { get; set; }
        public string? Compatibility { get; set; }
    }
}
