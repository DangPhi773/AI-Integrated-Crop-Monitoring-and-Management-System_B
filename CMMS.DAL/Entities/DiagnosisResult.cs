using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

public class DiagnosisResult
{
    [Key]
    public Guid DiagnosisResultId { get; set; }

    public Guid ReportId { get; set; }

    public Guid DiagnosedBy { get; set; }

    [Required]
    [MaxLength(255)]
    public string DiseaseName { get; set; } = null!;

    [Required]
    public string Conclusion { get; set; } = null!;

    [Required]
    public string RecommendedAction { get; set; } = null!;

    [MaxLength(50)]
    public string? SeverityLevel { get; set; }

    [MaxLength(50)]
    public string? Status { get; set; } = "DRAFT";

    [Column(TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("ReportId")]
    public virtual Report? Report { get; set; }

    [ForeignKey("DiagnosedBy")]
    public virtual User? Diagnoser { get; set; }

    public virtual ICollection<Recommendation> Recommendations { get; set; } = new List<Recommendation>();
}
