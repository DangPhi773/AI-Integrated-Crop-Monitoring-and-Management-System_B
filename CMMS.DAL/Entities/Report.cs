using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

[Table("Report")]
public partial class Report
{
    [Key]
    public Guid ReportId { get; set; }

    public Guid? WorkerId { get; set; }

    [MaxLength(255)]
    public string? Title { get; set; }

    public string? Description { get; set; }

    [MaxLength(50)]
    public string? Status { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "timestamp")]
    public DateTime? SubmitDate { get; set; }

    [ForeignKey("WorkerId")]
    public virtual User? Worker { get; set; }
}