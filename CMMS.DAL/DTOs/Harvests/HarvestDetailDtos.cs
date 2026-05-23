using System;
using System.ComponentModel.DataAnnotations;

namespace CMMS.DAL.DTOs.Harvests
{
    public class UpdateHarvestDetailRequest
    {
        [Range(0, int.MaxValue, ErrorMessage = "CropQuantity không được âm")]
        public int? CropQuantity { get; set; }

        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }

    public class RecordHarvestRequest
    {
        [Required(ErrorMessage = "ActualHarvestDate là bắt buộc")]
        public DateOnly ActualHarvestDate { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "ActualQuantity không được âm")]
        public int? ActualQuantity { get; set; }

        [Range(0.0, 1000000, ErrorMessage = "ActualWeightKg phải từ 0 đến 1,000,000")]
        public decimal? ActualWeightKg { get; set; }

        [StringLength(1000, ErrorMessage = "HarvestNotes tối đa 1000 ký tự")]
        public string? HarvestNotes { get; set; }
    }

    public class HarvestDetailResponse
    {
        public Guid HarvestDetailId { get; set; }
        public Guid HarvestId { get; set; }
        public Guid? BedId { get; set; }
        public string? BedName { get; set; }
        public Guid? CropId { get; set; }
        public string? CropName { get; set; }
        public Guid? PlotId { get; set; }
        public string? PlotName { get; set; }
        public Guid? SeasonId { get; set; }
        public string? SeasonName { get; set; }
        public int? CropQuantity { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public DateOnly? ActualHarvestDate { get; set; }
        public int? ActualQuantity { get; set; }
        public decimal? ActualWeightKg { get; set; }
        public string? HarvestNotes { get; set; }
        public bool IsHarvested => ActualHarvestDate.HasValue;
    }
}
