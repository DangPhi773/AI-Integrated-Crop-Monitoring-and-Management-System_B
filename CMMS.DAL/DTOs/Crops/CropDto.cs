using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMMS.DAL.DTOs.Crops
{
    public class CropRequest
    {
        [Required(ErrorMessage = "Tên cây trồng là bắt buộc")]
        [StringLength(150, MinimumLength = 1, ErrorMessage = "Tên cây trồng phải từ 1 đến 150 ký tự")]
        public string CropName { get; set; } = null!;

        [StringLength(200, ErrorMessage = "Tên khoa học tối đa 200 ký tự")]
        public string? CropScientificName { get; set; }

        [Range(1, 365, ErrorMessage = "Số ngày sinh trưởng phải từ 1 đến 365")]
        public int? CropDefaultGrowthDays { get; set; }

        [Range(0.01, 1000, ErrorMessage = "Khoảng cách cây phải từ 0.01 đến 1000")]
        public double? PlantSpacing { get; set; }

        [Range(0.01, 1000, ErrorMessage = "Bề rộng luống phải từ 0.01 đến 1000")]
        public double? BedWidthDefault { get; set; }

        [Range(0.01, 1000, ErrorMessage = "Bề rộng lối đi phải từ 0.01 đến 1000")]
        public double? PathWidthDefault { get; set; }

        [Range(1, 100, ErrorMessage = "Số hàng trên luống phải từ 1 đến 100")]
        public int? RowsPerBed { get; set; }

        [Range(0.01, 1000, ErrorMessage = "Khoảng cách hàng phải từ 0.01 đến 1000")]
        public double? RowSpacing { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng cây không được âm")]
        public int? CropQuantities { get; set; }

        [StringLength(50, ErrorMessage = "Status tối đa 50 ký tự")]
        public string? CropStatus { get; set; }
    }

    public class CropResponse
    {
        public Guid CropId { get; set; }
        public string CropName { get; set; } = null!;
        public string? CropScientificName { get; set; }
        public int? CropDefaultGrowthDays { get; set; }
        public double? PlantSpacing { get; set; }
        public double? BedWidthDefault { get; set; }
        public double? PathWidthDefault { get; set; }
        public int? RowsPerBed { get; set; }
        public double? RowSpacing { get; set; }
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
