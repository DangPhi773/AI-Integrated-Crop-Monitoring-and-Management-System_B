using System.ComponentModel.DataAnnotations;

namespace CMMS.DAL.DTOs.Expense;

public class CreateExpenseRequest
{
    [Required]
    public Guid SeasonId { get; set; }

    [Required, MaxLength(50)]
    public string Category { get; set; } = null!;

    [Required, MaxLength(255)]
    public string Description { get; set; } = null!;

    [Required, Range(typeof(decimal), "0.01", "999999999999.99")]
    public decimal Amount { get; set; }

    [Required]
    public DateOnly SpentAt { get; set; }

    public string? Notes { get; set; }
}
