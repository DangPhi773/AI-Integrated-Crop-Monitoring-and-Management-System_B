using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

[Table("RecommendationTask")]
public partial class RecommendationTask
{
    [Key]
    public Guid RecommendationTaskId { get; set; }

    public Guid? CreatedByOwnerId { get; set; }

    public Guid? AssignedToWorkerId { get; set; }

    [MaxLength(255)]
    public string? Title { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? TaskScheduledAt { get; set; }

    [MaxLength(50)]
    public string? TaskStatus { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? TaskCreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("AssignedToWorkerId")]
    public virtual User? AssignedToWorker { get; set; }

    [ForeignKey("CreatedByOwnerId")]
    public virtual User? CreatedByOwner { get; set; }

    public virtual ICollection<RecommendationTaskDetail> RecommendationTaskDetails { get; set; } = new List<RecommendationTaskDetail>();
}