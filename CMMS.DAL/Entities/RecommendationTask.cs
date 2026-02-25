using System;
using System.Collections.Generic;

namespace CMMS.DAL.Entities;

public partial class RecommendationTask
{
    public Guid RecommendationTaskId { get; set; }

    public Guid? CreatedByOwnerId { get; set; }

    public Guid? AssignedToWorkerId { get; set; }

    public string? Title { get; set; }

    public DateTime? TaskScheduledAt { get; set; }

    public string? TaskStatus { get; set; }

    public DateTime? TaskCreatedAt { get; set; }

    public virtual User? AssignedToWorker { get; set; }

    public virtual User? CreatedByOwner { get; set; }

    public virtual ICollection<RecommendationTaskDetail> RecommendationTaskDetails { get; set; } = new List<RecommendationTaskDetail>();
}
