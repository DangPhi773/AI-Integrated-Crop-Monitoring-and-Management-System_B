namespace CMMS.DAL.DTOs.Expense;

public class ExpenseResponse
{
    public Guid ExpenseId { get; set; }
    public Guid SeasonId { get; set; }
    public string? SeasonName { get; set; }
    public string Category { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Amount { get; set; }
    public DateOnly SpentAt { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}
