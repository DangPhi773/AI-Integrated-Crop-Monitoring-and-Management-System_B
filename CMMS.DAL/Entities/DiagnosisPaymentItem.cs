using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

public class DiagnosisPaymentItem
{
    [Key]
    public Guid DiagnosisPaymentItemId { get; set; }

    public Guid PaymentId { get; set; }

    public Guid DiagnosisResultId { get; set; }

    public Guid ContractId { get; set; }

    [ForeignKey("PaymentId")]
    public virtual DiagnosisPayment? Payment { get; set; }

    [ForeignKey("DiagnosisResultId")]
    public virtual DiagnosisResult? DiagnosisResult { get; set; }

    [ForeignKey("ContractId")]
    public virtual DiagnosisContract? Contract { get; set; }
}
