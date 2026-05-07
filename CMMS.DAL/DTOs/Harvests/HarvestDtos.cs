using System;
using System.Collections.Generic;

namespace CMMS.DAL.DTOs.Harvests
{
    public class CreateHarvestRequest
    {
        public Guid PlotId { get; set; }
        public Guid SeasonId { get; set; }
        public Guid CropId { get; set; }
        public DateOnly? ExpectedDate { get; set; }
        public decimal? ExpectedQuantity { get; set; }
        public string? Unit { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }

    public class UpdateHarvestRequest
    {
        public DateOnly? ExpectedDate { get; set; }
        public decimal? ExpectedQuantity { get; set; }
        public string? Unit { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
    }

    public class HarvestDetailDto
    {
        public Guid HarvestDetailId { get; set; }
        public Guid? BedId { get; set; }
        public string? BedName { get; set; }
        public int? CropQuantity { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public DateOnly? ActualHarvestDate { get; set; }
        public int? ActualQuantity { get; set; }
        public decimal? ActualWeightKg { get; set; }
        public string? HarvestNotes { get; set; }
    }

    public class HarvestResponse
    {
        public Guid HarvestId { get; set; }
        public Guid PlotId { get; set; }
        public string? PlotName { get; set; }
        public Guid SeasonId { get; set; }
        public string? SeasonName { get; set; }
        public Guid CropId { get; set; }
        public string? CropName { get; set; }
        public DateOnly? ExpectedDate { get; set; }
        public decimal? ExpectedQuantity { get; set; }
        public string? Unit { get; set; }
        public string Status { get; set; } = "planned";
        public string? Notes { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public List<HarvestDetailDto> HarvestDetails { get; set; } = new();
        public int HarvestedBedsCount { get; set; }
        public int? TotalHarvestedQuantity { get; set; }
        public decimal? TotalHarvestedWeightKg { get; set; }
    }

    public class HarvestSummary
    {
        public Guid HarvestId { get; set; }
        public Guid PlotId { get; set; }
        public string? PlotName { get; set; }
        public Guid CropId { get; set; }
        public string? CropName { get; set; }
        public Guid SeasonId { get; set; }
        public string? SeasonName { get; set; }
        public DateOnly? ExpectedDate { get; set; }
        public decimal? ExpectedQuantity { get; set; }
        public string? Unit { get; set; }
        public string Status { get; set; } = "planned";
        public int DetailsCount { get; set; }
        public int HarvestedBedsCount { get; set; }
    }
}
