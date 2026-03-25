using System;
using System.Collections.Generic;

namespace CMMS.DAL.Entities;

public partial class Crop
{
    public Guid CropId { get; set; }

    public Guid? SoilId { get; set; }

    public string CropName { get; set; } = null!;

    public string? CropScientificName { get; set; }

    public int? CropDefaultGrowthDays { get; set; }

    public int? CropQuantities { get; set; }

    public double? PlantSpacing { get; set; }

    public string? CropStatus { get; set; }

    public virtual ICollection<SoilCropCompatibility> SoilCropCompatibilities { get; set; } = new List<SoilCropCompatibility>();

    public virtual ICollection<SeasonsDetail> SeasonsDetails { get; set; } = new List<SeasonsDetail>();

    public virtual Soil? Soil { get; set; }
    public virtual ICollection<CropGrowthStage> CropGrowthStages { get; set; } = new List<CropGrowthStage>();
}
