using System;

namespace CMMS.DAL.DTOs.Harvests
{
    public class UpdateHarvestDetailRequest
    {
        public int? CropQuantity { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
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
    }
}
