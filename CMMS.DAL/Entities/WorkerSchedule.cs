using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

[Table("WorkerSchedule")]
public partial class WorkerSchedule
{
    [Key]
    public Guid ScheduleId { get; set; }

    public Guid? TaskDetailId { get; set; }

    public Guid? WorkerId { get; set; }

    public string? Description { get; set; }

    [MaxLength(50)]
    public string? Status { get; set; }

    [ForeignKey("TaskDetailId")]
    public virtual TaskDetail? TaskDetail { get; set; }

    [ForeignKey("WorkerId")]
    public virtual User? Worker { get; set; }
}