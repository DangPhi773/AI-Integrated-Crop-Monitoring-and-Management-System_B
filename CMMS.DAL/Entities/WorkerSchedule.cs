using System;
using System.Collections.Generic;

namespace CMMS.DAL.Entities;

public partial class WorkerSchedule
{
    public Guid ScheduleId { get; set; }

    public Guid? TaskDetailId { get; set; }

    public Guid? WorkerId { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public virtual TaskDetail? TaskDetail { get; set; }

    public virtual User? Worker { get; set; }
}
