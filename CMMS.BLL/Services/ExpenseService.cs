using CMMS.BLL.Helpers;
using CMMS.BLL.Interfaces;
using CMMS.DAL.DBContext;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Expense;
using CMMS.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CMMS.BLL.Services;

public class ExpenseService : IExpenseService
{
    private static readonly HashSet<string> AllowedCategories = new(StringComparer.OrdinalIgnoreCase)
    {
        "seed", "fertilizer", "pesticide", "labor", "equipment", "utility", "other"
    };

    private readonly AppDbContext _db;

    public ExpenseService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<ApiResponse<ExpenseResponse>> CreateAsync(CreateExpenseRequest request, Guid ownerId)
    {
        if (!AllowedCategories.Contains(request.Category))
            return Fail("Loại chi phí không hợp lệ");

        var season = await _db.Seasons.AsNoTracking().FirstOrDefaultAsync(s => s.SeasonId == request.SeasonId);
        if (season == null)
            return Fail("Không tìm thấy mùa vụ");

        var expense = new Expense
        {
            ExpenseId = Guid.NewGuid(),
            SeasonId = request.SeasonId,
            Category = request.Category.ToLowerInvariant(),
            Description = request.Description,
            Amount = request.Amount,
            SpentAt = request.SpentAt,
            Notes = request.Notes,
            CreatedBy = ownerId,
            CreatedAt = DateTimeHelper.VnNow()
        };

        _db.Expenses.Add(expense);
        await _db.SaveChangesAsync();

        return new ApiResponse<ExpenseResponse>
        {
            Success = true,
            Data = Map(expense, season.SeasonName),
            Message = "Tạo chi phí thành công"
        };
    }

    public async Task<ApiResponse<IEnumerable<ExpenseResponse>>> GetBySeasonAsync(Guid seasonId)
    {
        var items = await _db.Expenses
            .Include(e => e.Season)
            .AsNoTracking()
            .Where(e => e.SeasonId == seasonId)
            .OrderByDescending(e => e.SpentAt)
            .ToListAsync();

        return new ApiResponse<IEnumerable<ExpenseResponse>>
        {
            Success = true,
            Data = items.Select(e => Map(e, e.Season?.SeasonName))
        };
    }

    public async Task<ApiResponse<ExpenseResponse>> GetByIdAsync(Guid id)
    {
        var expense = await _db.Expenses
            .Include(e => e.Season)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.ExpenseId == id);
        if (expense == null)
            return Fail("Không tìm thấy chi phí");

        return new ApiResponse<ExpenseResponse>
        {
            Success = true,
            Data = Map(expense, expense.Season?.SeasonName)
        };
    }

    public async Task<ApiResponse<ExpenseResponse>> UpdateAsync(Guid id, UpdateExpenseRequest request)
    {
        if (!AllowedCategories.Contains(request.Category))
            return Fail("Loại chi phí không hợp lệ");

        var expense = await _db.Expenses
            .Include(e => e.Season)
            .FirstOrDefaultAsync(e => e.ExpenseId == id);
        if (expense == null)
            return Fail("Không tìm thấy chi phí");

        expense.Category = request.Category.ToLowerInvariant();
        expense.Description = request.Description;
        expense.Amount = request.Amount;
        expense.SpentAt = request.SpentAt;
        expense.Notes = request.Notes;

        await _db.SaveChangesAsync();

        return new ApiResponse<ExpenseResponse>
        {
            Success = true,
            Data = Map(expense, expense.Season?.SeasonName),
            Message = "Cập nhật chi phí thành công"
        };
    }

    public async Task<ApiResponse<string>> DeleteAsync(Guid id)
    {
        var expense = await _db.Expenses.FirstOrDefaultAsync(e => e.ExpenseId == id);
        if (expense == null)
            return new ApiResponse<string> { Success = false, Message = "Không tìm thấy chi phí" };

        _db.Expenses.Remove(expense);
        await _db.SaveChangesAsync();

        return new ApiResponse<string> { Success = true, Message = "Đã xoá chi phí" };
    }

    public async Task<ApiResponse<ExpenseSummaryResponse>> GetSummaryAsync(Guid seasonId)
    {
        var season = await _db.Seasons.AsNoTracking().FirstOrDefaultAsync(s => s.SeasonId == seasonId);
        if (season == null)
            return new ApiResponse<ExpenseSummaryResponse> { Success = false, Message = "Không tìm thấy mùa vụ" };

        var byCategory = await _db.Expenses
            .AsNoTracking()
            .Where(e => e.SeasonId == seasonId)
            .GroupBy(e => e.Category)
            .Select(g => new { Category = g.Key, Sum = g.Sum(x => x.Amount) })
            .ToListAsync();

        return new ApiResponse<ExpenseSummaryResponse>
        {
            Success = true,
            Data = new ExpenseSummaryResponse
            {
                SeasonId = seasonId,
                SeasonName = season.SeasonName,
                Total = byCategory.Sum(x => x.Sum),
                ByCategory = byCategory.ToDictionary(x => x.Category, x => x.Sum)
            }
        };
    }

    private static ApiResponse<ExpenseResponse> Fail(string message)
        => new() { Success = false, Message = message };

    private static ExpenseResponse Map(Expense e, string? seasonName) => new()
    {
        ExpenseId = e.ExpenseId,
        SeasonId = e.SeasonId,
        SeasonName = seasonName,
        Category = e.Category,
        Description = e.Description,
        Amount = e.Amount,
        SpentAt = e.SpentAt,
        Notes = e.Notes,
        CreatedAt = e.CreatedAt
    };
}
