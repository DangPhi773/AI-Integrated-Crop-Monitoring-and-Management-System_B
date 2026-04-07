using System;
using System.Collections.Generic;

namespace CMMS.DAL.DTOs.Beds
{
    public class BedAutoAllocateRequest
    {
        public Guid PlotId { get; set; }
        public Guid CropId { get; set; }
        public string? PlantingPattern { get; set; }
        public int? DesiredBedCount { get; set; }
        public string? BedNamePrefix { get; set; }
    }

    public class BedAllocationItem
    {
        public string BedName { get; set; } = null!;
        public double BedWidth { get; set; }
        public double BedLength { get; set; }
        public double BedArea { get; set; }
        public int RowCount { get; set; }
        public int PlantCount { get; set; }
    }

    public class BedAutoAllocateResponse
    {
        public Guid PlotId { get; set; }
        public Guid CropId { get; set; }
        public string PlantingPattern { get; set; } = null!;
        public List<string> AvailablePatterns { get; set; } = new();
        public double PlotAreaM2 { get; set; }
        public double EstimatedPlotSideM { get; set; }
        public double UsedAreaM2 { get; set; }
        public double UnusedAreaM2 { get; set; }
        public int BedCount { get; set; }
        public int TotalPlantCount { get; set; }
        public double DensityPerHa { get; set; }
        public bool DensityWarning { get; set; }
        public string? DensityWarningMessage { get; set; }
        public List<string> Warnings { get; set; } = new();
        public List<BedAllocationItem> Beds { get; set; } = new();
        public bool IsEstimatedShape { get; set; } = true;
    }
}
