namespace CMMS.DAL.DTOs.Expense;

public class ExpenseSummaryResponse
{
    public Guid SeasonId { get; set; }
    public string? SeasonName { get; set; }
    public decimal Total { get; set; }
    public Dictionary<string, decimal> ByCategory { get; set; } = new();
}
