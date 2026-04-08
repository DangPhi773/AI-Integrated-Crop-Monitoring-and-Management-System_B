using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

[Table("PestDetection")]
public partial class PestDetection
{
    [Key]
    public Guid PestDetectionId { get; set; }

    public Guid? SeasonId { get; set; }

    public Guid? SpecialistId { get; set; }

    public Guid? ImageAnalysisResultId { get; set; }

    [MaxLength(255)]
    public string? GeneralLabel { get; set; } 

    [MaxLength(50)]
    public string? GeneralSeverity { get; set; } 

    [MaxLength(100)]
    public string? ConfidenceSource { get; set; }

    [MaxLength(50)]
    public string? DetectionStatus { get; set; }

    public string? ReviewNotes { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? DetectedAt { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("ImageAnalysisResultId")]
    public virtual ImageAnalysisResult? ImageAnalysisResult { get; set; }

    [ForeignKey("SeasonId")]
    public virtual Season? Season { get; set; }

    [ForeignKey("SpecialistId")]
    public virtual User? Specialist { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Recommendation> Recommendations { get; set; } = new List<Recommendation>();
}