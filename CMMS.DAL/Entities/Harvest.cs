using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

[Table("Harvest")]
public partial class Harvest
{
    [Key]
    public Guid HarvestId { get; set; }

    [Required]
    public Guid PlotId { get; set; }

    [Required]
    public Guid SeasonId { get; set; }

    [Required]
    public Guid CropId { get; set; }

    [Column(TypeName = "date")]
    public DateOnly? ExpectedDate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? ExpectedQuantity { get; set; }

    [MaxLength(20)]
    public string? Unit { get; set; }

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "planned";

    public string? Notes { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "timestamp")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("PlotId")]
    public virtual Plot Plot { get; set; } = null!;

    [ForeignKey("SeasonId")]
    public virtual Season Season { get; set; } = null!;

    [ForeignKey("CropId")]
    public virtual Crop Crop { get; set; } = null!;

    public virtual ICollection<HarvestDetail> HarvestDetails { get; set; } = new List<HarvestDetail>();
}
