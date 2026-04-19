using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

[Table("Soil")]
public partial class Soil
{
    [Key]
    public Guid SoilId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = null!;

    [MaxLength(500)]
    public string? ScienceName { get; set; }

    public virtual ICollection<SoilCropCompatibility> SoilCropCompatibilities { get; set; } = new List<SoilCropCompatibility>();
    public virtual ICollection<Plot> Plots { get; set; } = new List<Plot>();
}