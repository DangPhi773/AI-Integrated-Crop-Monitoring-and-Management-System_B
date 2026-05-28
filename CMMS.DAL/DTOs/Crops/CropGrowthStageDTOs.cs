using System;
using System.ComponentModel.DataAnnotations;

namespace CMMS.DAL.DTOs.Crops
{
    public class CropGrowthStageRequest
    {
        [Required(ErrorMessage = "CropId là bắt buộc")]
        public Guid CropId { get; set; }

        [Required(ErrorMessage = "Tên giai đoạn là bắt buộc")]
        [StringLength(150, MinimumLength = 1, ErrorMessage = "Tên giai đoạn phải từ 1 đến 150 ký tự")]
        public string StageName { get; set; } = null!;

        [StringLength(1000, ErrorMessage = "Mô tả tối đa 1000 ký tự")]
        public string? StageDescription { get; set; }

        [Range(-50, 100, ErrorMessage = "Nhiệt độ tối thiểu phải từ -50 đến 100 °C")]
        public double? TemperatureMin { get; set; }

        [Range(0, 100, ErrorMessage = "Độ ẩm tối thiểu phải từ 0 đến 100 %")]
        public double? HumidityMin { get; set; }

        [Range(0, 100, ErrorMessage = "Độ ẩm đất tối thiểu phải từ 0 đến 100 %")]
        public double? SoilMoistureMin { get; set; }

        [StringLength(1000, ErrorMessage = "GrowthIndicators tối đa 1000 ký tự")]
        public string? GrowthIndicators { get; set; }

        [StringLength(1000, ErrorMessage = "CommonDiseases tối đa 1000 ký tự")]
        public string? CommonDiseases { get; set; }

        [StringLength(1000, ErrorMessage = "Notes tối đa 1000 ký tự")]
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
