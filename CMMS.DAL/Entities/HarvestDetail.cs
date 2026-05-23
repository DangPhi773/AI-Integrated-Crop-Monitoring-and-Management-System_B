using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

[Table("HarvestDetail")]
public partial class HarvestDetail
{
    [Key]
    public Guid HarvestDetailId { get; set; }

    [Required]
    public Guid HarvestId { get; set; }

    public Guid? BedId { get; set; }

    public int? CropQuantity { get; set; }

    [Column(TypeName = "date")]
    public DateOnly? StartDate { get; set; }

    [Column(TypeName = "date")]
    public DateOnly? EndDate { get; set; }

    [Column(TypeName = "date")]
    public DateOnly? ActualHarvestDate { get; set; }

    public int? ActualQuantity { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? ActualWeightKg { get; set; }

    public string? HarvestNotes { get; set; }

    [ForeignKey("HarvestId")]
    public virtual Harvest Harvest { get; set; } = null!;

    [ForeignKey("BedId")]
    public virtual Bed? Bed { get; set; }

    public virtual ICollection<GrowthTracking> GrowthTrackings { get; set; } = new List<GrowthTracking>();
}
