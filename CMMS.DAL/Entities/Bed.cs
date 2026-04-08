using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

[Table("Bed")]
public partial class Bed
{
    [Key]
    public Guid BedId { get; set; }

    public Guid? PlotId { get; set; }

    [MaxLength(200)]
    public string? BedName { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? BedArea { get; set; }

    [MaxLength(50)]
    public string? BedStatus { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? BedCreatedAt { get; set; }

    public int? CropQuantities { get; set; }

    [ForeignKey("PlotId")]
    public virtual Plot? Plot { get; set; }

    public virtual ICollection<SeasonsDetail> SeasonsDetails { get; set; } = new List<SeasonsDetail>();

    public virtual ICollection<IotDevice> IotDevices { get; set; } = new List<IotDevice>();
}