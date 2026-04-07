using System;

namespace CMMS.DAL.Entities;

public partial class CropBedConfig
{
    public Guid ConfigId { get; set; }
    public Guid CropId { get; set; }
    public string PlantingPattern { get; set; } = "straight"; // "straight" | "staggered"
    public double RowSpacing { get; set; }
    public double PlantSpacing { get; set; }
    public int RowsPerBed { get; set; }
    public double? BedWidthMin { get; set; }
    public double? BedWidthMax { get; set; }
    public double? PathWidthMin { get; set; }
    public double? PathWidthMax { get; set; }
    public double? BedHeight { get; set; }
    public int? DensityPerHaMin { get; set; }
    public int? DensityPerHaMax { get; set; }
    public bool IsDefault { get; set; }
    public string? Notes { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public virtual Crop? Crop { get; set; }
}
