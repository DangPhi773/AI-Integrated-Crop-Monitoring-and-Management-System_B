using System;
using System.ComponentModel.DataAnnotations;

namespace CMMS.DAL.DTOs.Crops
{
    public class SoilCropCompatibilityRequest
    {
        [Required(ErrorMessage = "SoilId là bắt buộc")]
        public Guid SoilId { get; set; }

        [Required(ErrorMessage = "CropId là bắt buộc")]
        public Guid CropId { get; set; }

        [StringLength(50, ErrorMessage = "Compatibility tối đa 50 ký tự")]
        public string? Compatibility { get; set; }

        [StringLength(500, ErrorMessage = "Note tối đa 500 ký tự")]
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
