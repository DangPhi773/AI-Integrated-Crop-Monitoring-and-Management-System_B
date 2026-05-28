using System;
using System.ComponentModel.DataAnnotations;

namespace CMMS.DAL.DTOs.Beds
{
    public class BedRequest
    {
        public Guid? PlotId { get; set; }

        [StringLength(150, MinimumLength = 1, ErrorMessage = "Tên luống phải từ 1 đến 150 ký tự")]
        public string? BedName { get; set; }

        [Range(0.0, 1000000, ErrorMessage = "Diện tích luống phải từ 0 đến 1,000,000")]
        public decimal? BedArea { get; set; }

        [StringLength(50, ErrorMessage = "Status tối đa 50 ký tự")]
        public string? BedStatus { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng cây không được âm")]
        public int? CropQuantities { get; set; }

        [Range(0.0, 1000, ErrorMessage = "Bề rộng luống phải từ 0 đến 1000")]
        public double? BedWidth { get; set; }

        [Range(0.0, 10000, ErrorMessage = "Chiều dài luống phải từ 0 đến 10,000")]
        public double? BedLength { get; set; }

        [Range(0.0, 1000, ErrorMessage = "Bề rộng lối đi phải từ 0 đến 1000")]
        public double? PathWidth { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số cây không được âm")]
        public int? PlantCount { get; set; }

        [Range(0, 1000, ErrorMessage = "Số hàng phải từ 0 đến 1000")]
        public int? RowCount { get; set; }
    }

    public class BedResponse
    {
        public Guid BedId { get; set; }
        public Guid? PlotId { get; set; }
        public string? BedName { get; set; }
        public decimal? BedArea { get; set; }
        public string? BedStatus { get; set; }
        public DateTime? BedCreatedAt { get; set; }
        public int? CropQuantities { get; set; }

        public double? BedWidth { get; set; }
        public double? BedLength { get; set; }
        public double? PathWidth { get; set; }
        public int? PlantCount { get; set; }
        public int? RowCount { get; set; }

        public string? PlotName { get; set; }
        public int HarvestDetailsCount { get; set; } = 0;
    }
}
