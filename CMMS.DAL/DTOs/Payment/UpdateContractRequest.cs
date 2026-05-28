using System.ComponentModel.DataAnnotations;

namespace CMMS.DAL.DTOs.Payment;

public class UpdateContractRequest
{
    [Required, MaxLength(50)]
    public string BankAccount { get; set; } = null!;

    [Required, MaxLength(100)]
    public string BankName { get; set; } = null!;

    [Required, RegularExpression("^[0-9]{6}$", ErrorMessage = "BankBin phải gồm 6 chữ số")]
    public string BankBin { get; set; } = null!;

    [Required, MaxLength(100)]
    public string AccountHolder { get; set; } = null!;

    public DateTime? EndDate { get; set; }

    public string? Notes { get; set; }
}
