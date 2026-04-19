using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

public class DiagnosisPayment
{
    [Key]
    public Guid Id { get; set; }

    public Guid PriceSettingId { get; set; }

    public int TotalDiagnoses { get; set; }

    [Column(TypeName = "decimal(12,2)")]
    public decimal Amount { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "pending";

    [MaxLength(20)]
    public string PaymentProvider { get; set; } = null!;

    [Column(TypeName = "jsonb")]
    public string? ProviderData { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? PaidAt { get; set; }

    [ForeignKey("PriceSettingId")]
    public virtual DiagnosisPriceSetting? PriceSetting { get; set; }
}
