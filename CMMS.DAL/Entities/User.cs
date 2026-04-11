using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

[Table("User")]
public partial class User
{
    [Key]
    public Guid UserId { get; set; }

    public Guid? RoleId { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = null!;

    [MaxLength(255)]
    public string? Password { get; set; }

    public string? HashPassword { get; set; }

    [MaxLength(255)]
    public string? Fullname { get; set; }

    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    [MaxLength(50)]
    public string? Status { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("RoleId")]
    public virtual Role? Role { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public virtual ICollection<RecommendationTask> RecommendationTaskAssignedToWorkers { get; set; } = new List<RecommendationTask>();
    public virtual ICollection<RecommendationTask> RecommendationTaskCreatedByOwners { get; set; } = new List<RecommendationTask>();
    public virtual ICollection<RecommendationTaskDetail> RecommendationTaskDetails { get; set; } = new List<RecommendationTaskDetail>();
    public virtual ICollection<Report> CreatedReports { get; set; } = new List<Report>();
    public virtual ICollection<Report> OwnedReports { get; set; } = new List<Report>();
    public virtual ICollection<WorkerSchedule> WorkerSchedules { get; set; } = new List<WorkerSchedule>();
}