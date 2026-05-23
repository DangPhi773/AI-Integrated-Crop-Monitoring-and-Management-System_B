using System;
using System.ComponentModel.DataAnnotations;

namespace CMMS.DAL.DTOs.Soils
{
    public class SoilRequest
    {
        [Required(ErrorMessage = "Tên loại đất là bắt buộc")]
        [StringLength(150, MinimumLength = 1, ErrorMessage = "Tên loại đất phải từ 1 đến 150 ký tự")]
        public string Name { get; set; } = null!;

        [StringLength(200, ErrorMessage = "Tên khoa học tối đa 200 ký tự")]
        public string? ScienceName { get; set; }
    }

    public class SoilResponse
    {
        public Guid SoilId { get; set; }
        public string Name { get; set; } = null!;
        public string? ScienceName { get; set; }

        public int CropsCount { get; set; } = 0;
        public int PlotsCount { get; set; } = 0;
    }
}
