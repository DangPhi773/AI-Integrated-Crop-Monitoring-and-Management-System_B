using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

[Table("ImageAnalysisResult")]
public partial class ImageAnalysisResult
{
    [Key]
    public Guid ImageAnalysisResultId { get; set; }

    public Guid? ImageAnalysisId { get; set; }

    [StringLength(255)]
    public string? AiLabel { get; set; }

    [Column(TypeName = "decimal(5,4)")]
    public decimal? AiConfidence { get; set; }

    [StringLength(50)]
    public string? AiSeverity { get; set; }

    [Column(TypeName = "text")]
    public string? BoundingBoxJson { get; set; }

    [Column(TypeName = "text")]
    public string? ExtraDataJson { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("ImageAnalysisId")]
    public virtual ImageAnalysis? ImageAnalysis { get; set; }

    public virtual ICollection<PestDetection> PestDetections { get; set; } = new List<PestDetection>();
}