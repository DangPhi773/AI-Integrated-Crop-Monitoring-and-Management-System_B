using System;
using System.Collections.Generic;

namespace CMMS.DAL.Entities;

public partial class Soil
{
    public Guid SoilId { get; set; }

    public string Name { get; set; } = null!;

    public string? ScienceName { get; set; }

    //public virtual ICollection<Crop> Crops { get; set; } = new List<Crop>();
    public virtual ICollection<SoilCropCompatibility> SoilCropCompatibilities { get; set; } = new List<SoilCropCompatibility>();

    public virtual ICollection<Plot> Plots { get; set; } = new List<Plot>();
}
