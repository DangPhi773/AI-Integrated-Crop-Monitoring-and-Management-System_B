using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

public class ReportAssignment
{
    [Key]
    public Guid Id { get; set; }

    public Guid ReportId { get; set; }

    public Guid AssignedBy { get; set; }

    public Guid AssignedTo { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime AssignedAt { get; set; }

    public string? Note { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = "ASSIGNED";

    [Column(TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("ReportId")]
    public virtual Report? Report { get; set; }

    [ForeignKey("AssignedBy")]
    public virtual User? Assigner { get; set; }

    [ForeignKey("AssignedTo")]
    public virtual User? Assignee { get; set; }
}
