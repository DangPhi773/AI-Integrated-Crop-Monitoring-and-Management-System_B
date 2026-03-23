//using System;
//using System.Collections.Generic;

//namespace CMMS.DAL.Entities;

//public partial class TaskDetail
//{
//    public Guid TaskDetailId { get; set; }

//    public Guid? TaskId { get; set; }

//    public Guid? SeasonId { get; set; }

//    public DateTime? StartDate { get; set; }

//    public DateTime? EndDate { get; set; }

//    public string? Notes { get; set; }

//    public virtual Season? Season { get; set; }

//    public virtual Task? Task { get; set; }

//    public virtual ICollection<WorkerSchedule> WorkerSchedules { get; set; } = new List<WorkerSchedule>();
//}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

[Table("task_details")]
public partial class TaskDetail
{
    [Key]
    [Column("task_detail_id")]
    public Guid TaskDetailId { get; set; }

    [Column("task_id")]
    public Guid? TaskId { get; set; }

    [Column("season_id")]
    public Guid? SeasonId { get; set; }

    [Column("assigned_to_worker_id")] 
    public Guid? AssignedToWorkerId { get; set; }

    [Column("start_date")]
    public DateTime? StartDate { get; set; }

    [Column("end_date")]
    public DateTime? EndDate { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    // Navigation properties
    public virtual Task? Task { get; set; }
    public virtual User? AssignedToWorker { get; set; }
    public virtual Season? Season { get; set; }
    public virtual ICollection<WorkerSchedule> WorkerSchedules { get; set; } = new List<WorkerSchedule>();
}