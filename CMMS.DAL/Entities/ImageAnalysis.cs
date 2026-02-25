using System;
using System.Collections.Generic;

namespace CMMS.DAL.Entities;

public partial class ImageAnalysis
{
    public Guid ImageAnalysisId { get; set; }

    public Guid? PhotoId { get; set; }

    public string? AnalysisType { get; set; }

    public string? AnalysisStatus { get; set; }

    public string? AiProvider { get; set; }

    public string? AiModel { get; set; }

    public string? AiModelVersion { get; set; }

    public string? AiPrompt { get; set; }

    public string? AiRequestId { get; set; }

    public int? AiTokens { get; set; }

    public int? AiLatencyMs { get; set; }

    public string? AiRawResponseJson { get; set; }

    public virtual ICollection<ImageAnalysisResult> ImageAnalysisResults { get; set; } = new List<ImageAnalysisResult>();

    public virtual Photo? Photo { get; set; }
}
