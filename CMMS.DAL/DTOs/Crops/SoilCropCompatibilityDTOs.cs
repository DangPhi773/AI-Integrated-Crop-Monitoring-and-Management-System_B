using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.DTOs.Crops
{
    public class SoilCropCompatibilityRequest
    {
        public Guid SoilId { get; set; }
        public Guid CropId { get; set; }
        public string? Compatibility { get; set; } 
        public string? Note { get; set; }
    }

    public class SoilCropCompatibilityResponse
    {
        public Guid ComptId { get; set; }
        public Guid SoilId { get; set; }
        public string? SoilName { get; set; } 
        public Guid CropId { get; set; }
        public string? CropName { get; set; }
        public string? Compatibility { get; set; }
        public string? Note { get; set; }
    }
}
