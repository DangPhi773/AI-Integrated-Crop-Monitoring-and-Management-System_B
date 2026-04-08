using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

[Table("ImageAnalysis")]
public partial class ImageAnalysis
{
    [Key]
    public Guid ImageAnalysisId { get; set; }

    public Guid? PhotoId { get; set; }

    [StringLength(100)]
    public string? AnalysisType { get; set; }

    [StringLength(50)]
    public string? AnalysisStatus { get; set; }

    [StringLength(100)]
    public string? AiProvider { get; set; }

    [StringLength(100)]
    public string? AiModel { get; set; }

    [StringLength(50)]
    public string? AiModelVersion { get; set; }

    public string? AiPrompt { get; set; }

    [StringLength(255)]
    public string? AiRequestId { get; set; }

    public int? AiTokens { get; set; }

    public int? AiLatencyMs { get; set; }

    [Column(TypeName = "text")] // Lưu JSON thô
    public string? AiRawResponseJson { get; set; }

    public virtual ICollection<ImageAnalysisResult> ImageAnalysisResults { get; set; } = new List<ImageAnalysisResult>();

    [ForeignKey("PhotoId")]
    public virtual Photo? Photo { get; set; }
}