using System;
using System.Collections.Generic;

namespace CMMS.DAL.Entities;

public partial class TaskDetail
{
    public Guid TaskDetailId { get; set; }

    public Guid? TaskId { get; set; }

    public Guid? SeasonId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Notes { get; set; }

    public virtual Season? Season { get; set; }

    public virtual Task? Task { get; set; }

    public virtual ICollection<WorkerSchedule> WorkerSchedules { get; set; } = new List<WorkerSchedule>();
}
