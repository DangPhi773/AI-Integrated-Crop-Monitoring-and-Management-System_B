using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

[Table("tasks")]
public partial class Task
{
    [Key]
    [Column("task_id")]
    public Guid TaskId { get; set; }

    [Column("task_title")]
    public string? TaskTitle { get; set; }

    [Column("task_scheduled_at")]
    public DateTime? TaskScheduledAt { get; set; }

    [Column("task_status")]
    public string? TaskStatus { get; set; }

    [Column("task_notes")]
    public string? TaskNotes { get; set; }

    [Column("task_created_at")]
    public DateTime? TaskCreatedAt { get; set; }

    // Navigation properties
    public virtual ICollection<TaskDetail> TaskDetails { get; set; } = new List<TaskDetail>();
}
