using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

public class DiagnosisPayment
{
    [Key]
    public Guid DiagnosisPaymentId { get; set; }

    public Guid SpecialistId { get; set; }

    [Column(TypeName = "date")]
    public DateTime Month { get; set; }

    public int TotalDiagnoses { get; set; }

    [Column(TypeName = "decimal(12,2)")]
    public decimal Amount { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "paid";

    [MaxLength(500)]
    public string? BillImageUrl { get; set; }

    [MaxLength(200)]
    public string? BillPublicId { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? PaidAt { get; set; }

    [ForeignKey("SpecialistId")]
    public virtual User? Specialist { get; set; }

    public virtual ICollection<DiagnosisPaymentItem> Items { get; set; } = new List<DiagnosisPaymentItem>();
}
