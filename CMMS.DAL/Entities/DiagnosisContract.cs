using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

public class DiagnosisContract
{
    [Key]
    public Guid DiagnosisContractId { get; set; }

    [MaxLength(30)]
    public string ContractCode { get; set; } = null!;

    public Guid ExpertId { get; set; }

    [MaxLength(50)]
    public string BankAccount { get; set; } = null!;

    [MaxLength(100)]
    public string BankName { get; set; } = null!;

    [MaxLength(100)]
    public string AccountHolder { get; set; } = null!;

    [Column(TypeName = "decimal(12,2)")]
    public decimal PricePerDiagnosis { get; set; }

    [Column(TypeName = "date")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "date")]
    public DateTime? EndDate { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "active";

    public string? Notes { get; set; }

    public Guid? CreatedBy { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("ExpertId")]
    public virtual User? Expert { get; set; }

    [ForeignKey("CreatedBy")]
    public virtual User? Creator { get; set; }
}
