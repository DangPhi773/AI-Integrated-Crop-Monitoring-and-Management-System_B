using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMMS.DAL.DTOs.Beds
{
    public class BedSplitRequest
    {
        [Required(ErrorMessage = "PlotId là bắt buộc")]
        public Guid PlotId { get; set; }

        [Required(ErrorMessage = "CropId là bắt buộc")]
        public Guid CropId { get; set; }

        [Range(0.01, 1000, ErrorMessage = "Bề rộng luống phải từ 0.01 đến 1000")]
        public double BedWidth { get; set; }

        [Range(0.0, 1000, ErrorMessage = "Bề rộng lối đi phải từ 0 đến 1000")]
        public double PathWidth { get; set; }

        [Range(1, 100, ErrorMessage = "Số hàng trên luống phải từ 1 đến 100")]
        public int RowsPerBed { get; set; }

        [StringLength(100, ErrorMessage = "BedNamePrefix tối đa 100 ký tự")]
        public string? BedNamePrefix { get; set; }
    }

    public class BedSplitConfirmRequest
    {
        [Required(ErrorMessage = "PlotId là bắt buộc")]
        public Guid PlotId { get; set; }

        [Required(ErrorMessage = "CropId là bắt buộc")]
        public Guid CropId { get; set; }

        [Required(ErrorMessage = "Danh sách luống là bắt buộc")]
        [MinLength(1, ErrorMessage = "Phải có ít nhất 1 luống")]
        public List<BedPreviewItem> Beds { get; set; } = new();
    }

    public class BedPreviewItem
    {
        [Required(ErrorMessage = "Tên luống là bắt buộc")]
        [StringLength(150, MinimumLength = 1, ErrorMessage = "Tên luống phải từ 1 đến 150 ký tự")]
        public string BedName { get; set; } = null!;

        [Range(0.0, 10000, ErrorMessage = "Chiều dài luống phải từ 0 đến 10,000")]
        public double BedLength { get; set; }

        [Range(0.0, 1000, ErrorMessage = "Bề rộng luống phải từ 0 đến 1000")]
        public double BedWidth { get; set; }

        [Range(0.0, 1000000, ErrorMessage = "Diện tích luống phải từ 0 đến 1,000,000")]
        public double BedArea { get; set; }

        [Range(0.0, 1000, ErrorMessage = "Bề rộng lối đi phải từ 0 đến 1000")]
        public double PathWidth { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số cây không được âm")]
        public int PlantCount { get; set; }

        [Range(0, 1000, ErrorMessage = "Số hàng phải từ 0 đến 1000")]
        public int RowCount { get; set; }

        [Required(ErrorMessage = "CropId là bắt buộc")]
        public Guid CropId { get; set; }
    }

    public class BedSplitPreview
    {
        public Guid PlotId { get; set; }
        public Guid CropId { get; set; }
        public int BedCount { get; set; }
        public double BedLength { get; set; }
        public double BedWidth { get; set; }
        public double BedArea { get; set; }
        public int PlantCount { get; set; }
        public double WidthRemain { get; set; }
        public List<BedPreviewItem> Beds { get; set; } = new();
    }
}
