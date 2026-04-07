using System;
using System.Collections.Generic;

namespace CMMS.DAL.Entities;

public partial class Plot
{
    public Guid PlotId { get; set; }

    public Guid? SoilId { get; set; }

    public Guid? FarmId { get; set; }

    public string? PlotName { get; set; }

    public decimal? PlotArea { get; set; }

    public double? PlotLength { get; set; }

    public double? PlotWidth { get; set; }

    public string? PlotStatus { get; set; }

    public DateTime? BedCreatedAt { get; set; }

    public virtual ICollection<Bed> Beds { get; set; } = new List<Bed>();

    public virtual Farm? Farm { get; set; }

    public virtual Soil? Soil { get; set; }
}
