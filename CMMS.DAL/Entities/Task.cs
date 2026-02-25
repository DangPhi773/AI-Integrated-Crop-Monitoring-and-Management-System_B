using System;
using System.Collections.Generic;

namespace CMMS.DAL.Entities;

public partial class Task
{
    public Guid TaskId { get; set; }

    public Guid? AssignedToWorkerId { get; set; }

    public Guid? SeasonId { get; set; }

    public string? TaskTitle { get; set; }

    public DateTime? TaskScheduledAt { get; set; }

    public string? TaskStatus { get; set; }

    public string? TaskNotes { get; set; }

    public DateTime? TaskCreatedAt { get; set; }

    public virtual User? AssignedToWorker { get; set; }

    public virtual Season? Season { get; set; }

    public virtual ICollection<TaskDetail> TaskDetails { get; set; } = new List<TaskDetail>();
}
