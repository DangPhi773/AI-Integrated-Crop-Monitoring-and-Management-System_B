using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

[Table("task_detail")]
public partial class TaskDetail
{
    [Key]
    [Column("task_detail_id")]
    public Guid TaskDetailId { get; set; }

    [Column("task_id")]
    public Guid? TaskId { get; set; }

    [Column("season_id")]
    public Guid? SeasonId { get; set; }

    [Column("farm_id")]
    public Guid? FarmId { get; set; }

    [Column("start_date", TypeName = "timestamp")]
    public DateTime? StartDate { get; set; }

    [Column("end_date", TypeName = "timestamp")]
    public DateTime? EndDate { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("assigned_to_worker_ids")]
    public List<Guid> AssignedToWorkerIds { get; set; } = new List<Guid>();

    [Column("plot_ids")]
    public List<Guid> PlotIds { get; set; } = new List<Guid>();

    [Column("bed_ids")]
    public List<Guid> BedIds { get; set; } = new List<Guid>();

    [ForeignKey("TaskId")]
    public virtual Task? Task { get; set; }

    [ForeignKey("SeasonId")]
    public virtual Season? Season { get; set; }

    [ForeignKey("FarmId")]
    public virtual Farm? Farm { get; set; }

    public virtual ICollection<WorkerSchedule> WorkerSchedules { get; set; } = new List<WorkerSchedule>();
    public virtual ICollection<SubTask> SubTasks { get; set; } = new List<SubTask>();
}