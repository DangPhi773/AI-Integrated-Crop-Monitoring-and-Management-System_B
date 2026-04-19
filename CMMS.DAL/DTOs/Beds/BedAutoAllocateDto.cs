using System;
using System.Collections.Generic;

namespace CMMS.DAL.DTOs.Beds
{
    public class BedSplitRequest
    {
        public Guid PlotId { get; set; }
        public Guid CropId { get; set; }
        public double BedWidth { get; set; }
        public double PathWidth { get; set; }
        public int RowsPerBed { get; set; }
        public string? BedNamePrefix { get; set; }
    }

    public class BedSplitConfirmRequest
    {
        public Guid PlotId { get; set; }
        public Guid CropId { get; set; }
        public List<BedPreviewItem> Beds { get; set; } = new();
    }

    public class BedPreviewItem
    {
        public string BedName { get; set; } = null!;
        public double BedLength { get; set; }
        public double BedWidth { get; set; }
        public double BedArea { get; set; }
        public double PathWidth { get; set; }
        public int PlantCount { get; set; }
        public int RowCount { get; set; }
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
