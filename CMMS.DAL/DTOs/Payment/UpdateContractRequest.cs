using System.ComponentModel.DataAnnotations;

namespace CMMS.DAL.DTOs.Payment;

public class UpdateContractRequest
{
    [Required, MaxLength(50)]
    public string BankAccount { get; set; } = null!;

    [Required, MaxLength(100)]
    public string BankName { get; set; } = null!;

    [Required, MaxLength(100)]
    public string AccountHolder { get; set; } = null!;

    [Required, Range(0.01, double.MaxValue)]
    public decimal PricePerDiagnosis { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Notes { get; set; }
}
