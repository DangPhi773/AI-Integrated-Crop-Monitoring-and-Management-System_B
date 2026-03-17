using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.DTOs.Crops
{
    public class CropRequest
    {
        public Guid? SoilId { get; set; }
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
        public Guid? SoilId { get; set; }
        public string CropName { get; set; } = null!;
        public string? CropScientificName { get; set; }
        public int? CropDefaultGrowthDays { get; set; }
        public double? PlantSpacing { get; set; }
        public int? CropQuantities { get; set; }
        public string? CropStatus { get; set; }



        public string? SoilName { get; set; }
        public string? SoilScienceName { get; set; }
    }
}
