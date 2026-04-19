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

    [MaxLength(50)]
    public string? ReportNo { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? OwnerId { get; set; }

    [MaxLength(255)]
    public string? Title { get; set; }

    public string? Description { get; set; }

    [MaxLength(50)]
    public string? ReportType { get; set; }

    public Guid? PlotId { get; set; }

    public Guid? BedId { get; set; }

    public Guid? SeasonId { get; set; }

    [Column(TypeName = "jsonb")]
    public string? AiResultsJson { get; set; }

    [MaxLength(50)]
    public string? Status { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? SubmitDate { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("CreatedBy")]
    public virtual User? Creator { get; set; }

    [ForeignKey("OwnerId")]
    public virtual User? Owner { get; set; }

    [ForeignKey("PlotId")]
    public virtual Plot? Plot { get; set; }

    [ForeignKey("BedId")]
    public virtual Bed? Bed { get; set; }

    [ForeignKey("SeasonId")]
    public virtual Season? Season { get; set; }

    public virtual ICollection<ReportAssignment> ReportAssignments { get; set; } = new List<ReportAssignment>();
    public virtual ICollection<DiagnosisResult> DiagnosisResults { get; set; } = new List<DiagnosisResult>();
    public virtual ICollection<ReportEnvironmentSnapshot> EnvironmentSnapshots { get; set; } = new List<ReportEnvironmentSnapshot>();
}
