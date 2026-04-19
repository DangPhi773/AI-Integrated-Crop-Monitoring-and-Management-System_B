using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

[Table("Plot")]
public partial class Plot
{
    [Key]
    public Guid PlotId { get; set; }

    public Guid? SoilId { get; set; }

    public Guid? FarmId { get; set; }

    [MaxLength(255)]
    public string? PlotName { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? PlotArea { get; set; }

    public double? PlotLength { get; set; }

    public double? PlotWidth { get; set; }

    public double PlotMargin { get; set; } = 0.3;

    [MaxLength(50)]
    public string? PlotStatus { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? BedCreatedAt { get; set; }

    public virtual ICollection<Bed> Beds { get; set; } = new List<Bed>();

    [ForeignKey("FarmId")]
    public virtual Farm? Farm { get; set; }

    [ForeignKey("SoilId")]
    public virtual Soil? Soil { get; set; }
}