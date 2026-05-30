using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Expense;

namespace CMMS.BLL.Interfaces;

public interface IExpenseService
{
    Task<ApiResponse<ExpenseResponse>> CreateAsync(CreateExpenseRequest request, Guid ownerId);
    Task<ApiResponse<IEnumerable<ExpenseResponse>>> GetBySeasonAsync(Guid seasonId);
    Task<ApiResponse<ExpenseResponse>> GetByIdAsync(Guid id);
    Task<ApiResponse<ExpenseResponse>> UpdateAsync(Guid id, UpdateExpenseRequest request);
    Task<ApiResponse<string>> DeleteAsync(Guid id);
    Task<ApiResponse<ExpenseSummaryResponse>> GetSummaryAsync(Guid seasonId);
}
