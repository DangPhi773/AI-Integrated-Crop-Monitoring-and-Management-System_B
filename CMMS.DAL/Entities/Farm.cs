using System;
using System.Collections.Generic;

namespace CMMS.DAL.Entities;

public partial class Farm
{
    public Guid FarmId { get; set; }

    public string FarmName { get; set; } = null!;

    public string? FarmLocation { get; set; }

    public decimal? FarmArea { get; set; }

    public string? FarmStatus { get; set; }

    public DateTime? FarmCreatedAt { get; set; }

    public virtual ICollection<Plot> Plots { get; set; } = new List<Plot>();

    public virtual ICollection<Season> Seasons { get; set; } = new List<Season>();
}
