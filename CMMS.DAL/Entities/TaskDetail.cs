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

    [Column("start_date")]
    public DateTime? StartDate { get; set; }

    [Column("end_date")]
    public DateTime? EndDate { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    // --- CÁC CỘT DẠNG MẢNG (ARRAY) ---

    [Column("assigned_to_worker_ids")]
    public List<Guid> AssignedToWorkerIds { get; set; } = new List<Guid>();

    [Column("plot_ids")]
    public List<Guid> PlotIds { get; set; } = new List<Guid>();

    [Column("bed_ids")]
    public List<Guid> BedIds { get; set; } = new List<Guid>();

    // --- NAVIGATION PROPERTIES ---
    public virtual Task? Task { get; set; }
    public virtual Season? Season { get; set; }

    // Lưu ý: Không tạo virtual User hay virtual Bed ở đây 
    // vì EF Core không hỗ trợ mapping 1 mảng ID sang Navigation.
    public virtual ICollection<WorkerSchedule> WorkerSchedules { get; set; } = new List<WorkerSchedule>();
    public virtual ICollection<SubTask> SubTasks { get; set; } = new List<SubTask>();
}