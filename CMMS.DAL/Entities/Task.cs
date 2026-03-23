//using System;
//using System.Collections.Generic;

//namespace CMMS.DAL.Entities;

//public partial class Task
//{
//    public Guid TaskId { get; set; }

//    public Guid? AssignedToWorkerId { get; set; }

//    public Guid? SeasonId { get; set; }

//    public string? TaskTitle { get; set; }

//    public DateTime? TaskScheduledAt { get; set; }

//    public string? TaskStatus { get; set; }

//    public string? TaskNotes { get; set; }

//    public DateTime? TaskCreatedAt { get; set; }

//    public virtual User? AssignedToWorker { get; set; }

//    public virtual Season? Season { get; set; }

//    public virtual ICollection<TaskDetail> TaskDetails { get; set; } = new List<TaskDetail>();
//}
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