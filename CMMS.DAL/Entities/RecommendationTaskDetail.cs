using CMMS.DAL.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("RecommendationTaskDetail")]
public partial class RecommendationTaskDetail
{
    [Key]
    public Guid TaskDetailId { get; set; }

    public Guid? TaskId { get; set; }

    public Guid? SeasonId { get; set; }
    public Guid? FarmId { get; set; }

    [MaxLength(255)]
    public string? Title { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? Quantity { get; set; }

    [MaxLength(50)]
    public string? Status { get; set; } = "Pending";

    [MaxLength(50)]
    public string? Unit { get; set; }

    public string? Notes { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? StartDate { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? EndDate { get; set; }

    public List<Guid> AssignedToWorkerIds { get; set; } = new List<Guid>();
    public List<Guid> PlotIds { get; set; } = new List<Guid>();
    public List<Guid> BedIds { get; set; } = new List<Guid>();

    [ForeignKey("TaskId")]
    public virtual RecommendationTask? Task { get; set; }

    [ForeignKey("SeasonId")]
    public virtual Season? Season { get; set; }

    [ForeignKey("FarmId")]
    public virtual Farm? Farm { get; set; }

    public virtual ICollection<WorkerSchedule> WorkerSchedules { get; set; } = new List<WorkerSchedule>();
}