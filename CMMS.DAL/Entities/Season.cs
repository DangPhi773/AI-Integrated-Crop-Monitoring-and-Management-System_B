using System;
using System.Collections.Generic;

namespace CMMS.DAL.Entities;

public partial class Season
{
    public Guid SeasonId { get; set; }

    public Guid? FarmId { get; set; }

    public string? SeasonName { get; set; }

    public DateOnly? SeasonStartDate { get; set; }

    public DateOnly? SeasonEndDate { get; set; }

    public string? Description { get; set; }

    public string? SeasonNotes { get; set; }

    public DateTime? SeasonCreatedAt { get; set; }

    public string? Status { get; set; }

    public virtual Farm? Farm { get; set; }

    public virtual ICollection<PestDetection> PestDetections { get; set; } = new List<PestDetection>();

    public virtual ICollection<Recommendation> Recommendations { get; set; } = new List<Recommendation>();

    public virtual ICollection<SeasonsDetail> SeasonsDetails { get; set; } = new List<SeasonsDetail>();

    public virtual ICollection<TaskDetail> TaskDetails { get; set; } = new List<TaskDetail>();

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
