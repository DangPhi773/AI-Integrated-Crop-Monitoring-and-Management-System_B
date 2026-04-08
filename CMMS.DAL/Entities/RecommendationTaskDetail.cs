using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

[Table("RecommendationTaskDetail")]
public partial class RecommendationTaskDetail
{
    [Key]
    public Guid TaskDetailId { get; set; }

    public Guid? TaskId { get; set; }

    public Guid? WorkerId { get; set; }

    [MaxLength(255)]
    public string? Title { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? Quantity { get; set; }

    [MaxLength(50)]
    public string? Status { get; set; }

    [MaxLength(50)]
    public string? Unit { get; set; }

    public string? Notes { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? StartDate { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? EndDate { get; set; }

    [ForeignKey("TaskId")]
    public virtual RecommendationTask? Task { get; set; }

    [ForeignKey("WorkerId")]
    public virtual User? Worker { get; set; }
}