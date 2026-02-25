using System;
using System.Collections.Generic;

namespace CMMS.DAL.Entities;

public partial class Bed
{
    public Guid BedId { get; set; }

    public Guid? PlotId { get; set; }

    public string? BedName { get; set; }

    public decimal? BedArea { get; set; }

    public string? BedStatus { get; set; }

    public DateTime? BedCreatedAt { get; set; }

    public int? CropQuantities { get; set; }

    public virtual Plot? Plot { get; set; }

    public virtual ICollection<SeasonsDetail> SeasonsDetails { get; set; } = new List<SeasonsDetail>();
}
