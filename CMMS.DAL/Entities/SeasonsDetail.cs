using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

[Table("SeasonsDetail")]
public partial class SeasonsDetail
{
    [Key]
    public Guid SeasonDetailId { get; set; }

    public Guid? SeasonId { get; set; }

    public Guid? BedId { get; set; }

    public Guid? CropId { get; set; }

    [Column(TypeName = "date")]
    public DateOnly? SeasonExpectedHarvestDate { get; set; }

    public int? CropQuantity { get; set; }

    [Column(TypeName = "date")]
    public DateOnly? StartDate { get; set; }

    [Column(TypeName = "date")]
    public DateOnly? EndDate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? TotalHarvestYield { get; set; }

    [ForeignKey("BedId")]
    public virtual Bed? Bed { get; set; }

    [ForeignKey("CropId")]
    public virtual Crop? Crop { get; set; }

    [ForeignKey("SeasonId")]
    public virtual Season? Season { get; set; }

    public virtual ICollection<GrowthTracking> GrowthTrackings { get; set; } = new List<GrowthTracking>();
}