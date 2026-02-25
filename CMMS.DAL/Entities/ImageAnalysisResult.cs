using System;
using System.Collections.Generic;

namespace CMMS.DAL.Entities;

public partial class ImageAnalysisResult
{
    public Guid ImageAnalysisResultId { get; set; }

    public Guid? ImageAnalysisId { get; set; }

    public string? AiLabel { get; set; }

    public decimal? AiConfidence { get; set; }

    public string? AiSeverity { get; set; }

    public string? BoundingBoxJson { get; set; }

    public string? ExtraDataJson { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ImageAnalysis? ImageAnalysis { get; set; }

    public virtual ICollection<PestDetection> PestDetections { get; set; } = new List<PestDetection>();
}
