using System;
using System.Collections.Generic;

namespace CMMS.DAL.Entities;

public partial class PestDetection
{
    public Guid PestDetectionId { get; set; }

    public Guid? SeasonId { get; set; }

    public Guid? SpecialistId { get; set; }

    public Guid? ImageAnalysisResultId { get; set; }

    public string? GeneralLabel { get; set; }

    public string? GeneralSeverity { get; set; }

    public string? ConfidenceSource { get; set; }

    public string? DetectionStatus { get; set; }

    public string? ReviewNotes { get; set; }

    public DateTime? DetectedAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ImageAnalysisResult? ImageAnalysisResult { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Recommendation> Recommendations { get; set; } = new List<Recommendation>();

    public virtual Season? Season { get; set; }

    public virtual User? Specialist { get; set; }
}
