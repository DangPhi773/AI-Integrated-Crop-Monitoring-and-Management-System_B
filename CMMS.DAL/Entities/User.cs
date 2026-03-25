using System;
using System.Collections.Generic;

namespace CMMS.DAL.Entities;

public partial class User
{
    public Guid UserId { get; set; }

    public Guid? RoleId { get; set; }

    public string Email { get; set; } = null!;

    public string? Password { get; set; }
    public string? HashPassword { get; set; }

    public string? Fullname { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<PestDetection> PestDetections { get; set; } = new List<PestDetection>();

    public virtual ICollection<RecommendationTask> RecommendationTaskAssignedToWorkers { get; set; } = new List<RecommendationTask>();

    public virtual ICollection<RecommendationTask> RecommendationTaskCreatedByOwners { get; set; } = new List<RecommendationTask>();

    public virtual ICollection<RecommendationTaskDetail> RecommendationTaskDetails { get; set; } = new List<RecommendationTaskDetail>();

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<TaskDetail> TaskDetails { get; set; } = new List<TaskDetail>();

    public virtual ICollection<WorkerSchedule> WorkerSchedules { get; set; } = new List<WorkerSchedule>();
}