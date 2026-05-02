using System;

namespace CMMS.DAL.DTOs.Beds
{
    public class BedRequest
    {
        public Guid? PlotId { get; set; }
        public string? BedName { get; set; }
        public decimal? BedArea { get; set; }
        public string? BedStatus { get; set; }
        public int? CropQuantities { get; set; }

        public double? BedWidth { get; set; }
        public double? BedLength { get; set; }
        public double? PathWidth { get; set; }
        public int? PlantCount { get; set; }
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
