using System;
using System.ComponentModel.DataAnnotations;

namespace CMMS.DAL.DTOs.Plots
{
    public class PlotRequest
    {
        [Required(ErrorMessage = "Mã trang trại (FarmId) là bắt buộc")] 
        public Guid? FarmId { get; set; }

        [Required(ErrorMessage = "Mã loại đất (SoilId) là bắt buộc")]
        public Guid? SoilId { get; set; }

        [StringLength(150, MinimumLength = 1, ErrorMessage = "Tên thửa phải từ 1 đến 150 ký tự")]
        public string? PlotName { get; set; }

        [Range(0.0, 1000000, ErrorMessage = "Diện tích thửa phải từ 0 đến 1,000,000")]
        public decimal? PlotArea { get; set; }

        [Range(0.0, 10000, ErrorMessage = "Chiều dài thửa phải từ 0 đến 10,000")]
        public double? PlotLength { get; set; }

        [Range(0.0, 10000, ErrorMessage = "Chiều rộng thửa phải từ 0 đến 10,000")]
        public double? PlotWidth { get; set; }

        [Range(0.0, 1000, ErrorMessage = "Khoảng lề chiều dài phải từ 0 đến 1000")]
        public double? PlotMarginLength { get; set; }

        [Range(0.0, 1000, ErrorMessage = "Khoảng lề chiều rộng phải từ 0 đến 1000")]
        public double? PlotMarginWidth { get; set; }

        [StringLength(50, ErrorMessage = "Status tối đa 50 ký tự")]
        public string? PlotStatus { get; set; }
    }

    public class PlotResponse
    {
        public Guid PlotId { get; set; }
        public Guid? FarmId { get; set; }
        public Guid? SoilId { get; set; }
        public string? PlotName { get; set; }
        public decimal? PlotArea { get; set; }
        public double? PlotLength { get; set; }
        public double? PlotWidth { get; set; }
        public double PlotMarginLength { get; set; }
        public double PlotMarginWidth { get; set; }
        public string? PlotStatus { get; set; }
        public DateTime? PlotCreatedAt { get; set; }

        public string? FarmName { get; set; }
        public string? SoilName { get; set; }
        public int BedsCount { get; set; } = 0;
    }
}
