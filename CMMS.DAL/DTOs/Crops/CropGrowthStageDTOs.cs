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

        [Range(-50, 100, ErrorMessage = "Nhiệt độ tối đa phải từ -50 đến 100 °C")]
        public double? TemperatureMax { get; set; }

        [Range(0, 100, ErrorMessage = "Độ ẩm tối đa phải từ 0 đến 100 %")]
        public double? HumidityMax { get; set; }

        [Range(0, 100, ErrorMessage = "Độ ẩm đất tối đa phải từ 0 đến 100 %")]
        public double? SoilMoistureMax { get; set; }

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
        public double? TemperatureMax { get; set; }
        public double? HumidityMax { get; set; }
        public double? SoilMoistureMax { get; set; }
        public string? GrowthIndicators { get; set; }
        public string? CommonDiseases { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
