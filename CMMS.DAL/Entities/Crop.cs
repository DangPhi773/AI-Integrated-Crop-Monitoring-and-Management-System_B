using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

[Table("Crop")]
public partial class Crop
{
    [Key]
    public Guid CropId { get; set; }

    [Required]
    [MaxLength(200)]
    public string CropName { get; set; } = null!;

    [MaxLength(500)]
    public string? CropScientificName { get; set; }

    public int? CropDefaultGrowthDays { get; set; }

    public int? CropQuantities { get; set; }

    public double? PlantSpacing { get; set; }

    [MaxLength(50)]
    public string? CropStatus { get; set; }

    public virtual ICollection<SoilCropCompatibility> SoilCropCompatibilities { get; set; } = new List<SoilCropCompatibility>();

    public virtual ICollection<SeasonsDetail> SeasonsDetails { get; set; } = new List<SeasonsDetail>();

    public virtual ICollection<CropGrowthStage> CropGrowthStages { get; set; } = new List<CropGrowthStage>();
}
