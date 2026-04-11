using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

[Table("Recommendation")]
public partial class Recommendation
{
    [Key]
    public Guid RecommendationId { get; set; }

    public Guid? SeasonId { get; set; }

    public Guid? DiagnosisId { get; set; }

    [MaxLength(255)]
    public string? Title { get; set; }

    public string? Content { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("DiagnosisId")]
    public virtual DiagnosisResult? Diagnosis { get; set; }

    [ForeignKey("SeasonId")]
    public virtual Season? Season { get; set; }
}
