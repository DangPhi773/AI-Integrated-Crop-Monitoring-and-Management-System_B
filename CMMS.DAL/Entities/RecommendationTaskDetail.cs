using System;
using System.Collections.Generic;

namespace CMMS.DAL.Entities;

public partial class RecommendationTaskDetail
{
    public Guid TaskDetailId { get; set; }

    public Guid? TaskId { get; set; }

    public Guid? WorkerId { get; set; }

    public string? Title { get; set; }

    public decimal? Quantity { get; set; }

    public string? Status { get; set; }

    public string? Unit { get; set; }

    public string? Notes { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public virtual RecommendationTask? Task { get; set; }

    public virtual User? Worker { get; set; }
}
