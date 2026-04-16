using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

public class DiagnosisPriceSetting
{
    [Key]
    public Guid Id { get; set; }

    public Guid FarmId { get; set; }

    public Guid ExpertId { get; set; }

    [Column(TypeName = "date")]
    public DateTime Month { get; set; }

    [Column(TypeName = "decimal(12,2)")]
    public decimal PricePerDiagnosis { get; set; }

    public string? Notes { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    [ForeignKey("FarmId")]
    public virtual Farm? Farm { get; set; }

    [ForeignKey("ExpertId")]
    public virtual User? Expert { get; set; }

    [ForeignKey("CreatedBy")]
    public virtual User? Creator { get; set; }

    public virtual ICollection<DiagnosisPayment> Payments { get; set; } = new List<DiagnosisPayment>();
}
