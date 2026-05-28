using CMMS.BLL.Helpers;
using CMMS.BLL.Interfaces;
using CMMS.DAL.DBContext;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Payment;
using CMMS.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CMMS.BLL.Services;

public class DiagnosisContractService : IDiagnosisContractService
{
    private readonly AppDbContext _db;

    public DiagnosisContractService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<ApiResponse<ContractResponse>> CreateAsync(CreateContractRequest request, Guid ownerId)
    {
        var expert = await _db.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.UserId == request.ExpertId);
        if (expert == null)
            return Fail("Không tìm thấy chuyên gia");
        if (expert.Role?.RoleName != "Specialist")
            return Fail("Người được chọn không phải chuyên gia");

        if (request.EndDate.HasValue && request.EndDate.Value.Date <= request.StartDate.Date)
            return Fail("Ngày kết thúc phải sau ngày bắt đầu");

        var existingActive = await _db.DiagnosisContracts
            .FirstOrDefaultAsync(c => c.ExpertId == request.ExpertId && c.Status == "active");

        if (existingActive != null)
        {
            if (request.StartDate.Date <= existingActive.StartDate.Date)
                return Fail("Ngày bắt đầu phải sau ngày bắt đầu của hợp đồng đang hiệu lực");

            existingActive.Status = "terminated";
            existingActive.EndDate = request.StartDate.Date.AddDays(-1);
        }

        var now = DateTimeHelper.VnNow();
        var prefix = $"HD-{now:yyyyMM}";
        var seq = await _db.DiagnosisContracts.CountAsync(c => c.ContractCode.StartsWith(prefix)) + 1;
        var code = $"{prefix}-{seq:D3}";

        var contract = new DiagnosisContract
        {
            DiagnosisContractId = Guid.NewGuid(),
            ContractCode = code,
            ExpertId = request.ExpertId,
            BankAccount = request.BankAccount,
            BankName = request.BankName,
            BankBin = request.BankBin,
            AccountHolder = request.AccountHolder,
            PricePerDiagnosis = request.PricePerDiagnosis,
            StartDate = request.StartDate.Date,
            EndDate = request.EndDate?.Date,
            Status = "active",
            Notes = request.Notes,
            CreatedBy = ownerId,
            CreatedAt = now
        };

        _db.DiagnosisContracts.Add(contract);
        await _db.SaveChangesAsync();

        var response = await BuildResponse(contract.DiagnosisContractId);
        return new ApiResponse<ContractResponse>
        {
            Success = true,
            Data = response,
            Message = "Tạo hợp đồng thành công"
        };
    }

    public async Task<ApiResponse<ContractResponse>> GetByIdAsync(Guid id, Guid userId, string role)
    {
        var contract = await _db.DiagnosisContracts
            .Include(c => c.Expert)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.DiagnosisContractId == id);

        if (contract == null || !CanAccess(contract, userId, role))
            return new ApiResponse<ContractResponse> { Success = false, Message = "Không tìm thấy hợp đồng" };

        return new ApiResponse<ContractResponse> { Success = true, Data = Map(contract) };
    }

    public async Task<ApiResponse<IEnumerable<ContractResponse>>> GetMyContractsAsync(Guid userId, string role)
    {
        var query = _db.DiagnosisContracts
            .Include(c => c.Expert)
            .AsNoTracking()
            .OrderByDescending(c => c.CreatedAt)
            .AsQueryable();

        query = role switch
        {
            "Owner" => query.Where(c => c.CreatedBy == userId),
            "Specialist" => query.Where(c => c.ExpertId == userId),
            _ => query.Where(c => false)
        };

        var items = await query.ToListAsync();
        return new ApiResponse<IEnumerable<ContractResponse>>
        {
            Success = true,
            Data = items.Select(Map)
        };
    }

    public async Task<ApiResponse<IEnumerable<ContractResponse>>> GetAllAsync()
    {
        var items = await _db.DiagnosisContracts
            .Include(c => c.Expert)
            .AsNoTracking()
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        return new ApiResponse<IEnumerable<ContractResponse>>
        {
            Success = true,
            Data = items.Select(Map),
            Message = $"Lấy được {items.Count} hợp đồng"
        };
    }

    public async Task<ApiResponse<ContractResponse>> UpdateAsync(Guid id, UpdateContractRequest request, Guid ownerId)
    {
        var contract = await _db.DiagnosisContracts.FirstOrDefaultAsync(c => c.DiagnosisContractId == id);
        if (contract == null || contract.CreatedBy != ownerId)
            return new ApiResponse<ContractResponse> { Success = false, Message = "Không tìm thấy hợp đồng" };
        if (contract.Status != "active")
            return new ApiResponse<ContractResponse> { Success = false, Message = "Hợp đồng đã kết thúc, không thể sửa" };

        if (request.EndDate.HasValue && request.EndDate.Value.Date <= contract.StartDate)
            return new ApiResponse<ContractResponse> { Success = false, Message = "Ngày kết thúc phải sau ngày bắt đầu" };

        contract.BankAccount = request.BankAccount;
        contract.BankName = request.BankName;
        contract.BankBin = request.BankBin;
        contract.AccountHolder = request.AccountHolder;
        contract.EndDate = request.EndDate?.Date;
        contract.Notes = request.Notes;

        await _db.SaveChangesAsync();

        var response = await BuildResponse(id);
        return new ApiResponse<ContractResponse>
        {
            Success = true,
            Data = response,
            Message = "Cập nhật hợp đồng thành công"
        };
    }

    public async Task<ApiResponse<string>> TerminateAsync(Guid id, Guid ownerId)
    {
        var contract = await _db.DiagnosisContracts.FirstOrDefaultAsync(c => c.DiagnosisContractId == id);
        if (contract == null || contract.CreatedBy != ownerId)
            return new ApiResponse<string> { Success = false, Message = "Không tìm thấy hợp đồng" };
        if (contract.Status == "terminated")
            return new ApiResponse<string> { Success = false, Message = "Hợp đồng đã kết thúc trước đó" };

        contract.Status = "terminated";
        contract.EndDate = DateTimeHelper.VnNow().Date;
        await _db.SaveChangesAsync();

        return new ApiResponse<string> { Success = true, Message = "Đã kết thúc hợp đồng" };
    }

    private async Task<ContractResponse?> BuildResponse(Guid id)
    {
        var c = await _db.DiagnosisContracts
            .Include(x => x.Expert)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.DiagnosisContractId == id);
        return c == null ? null : Map(c);
    }

    private static ContractResponse Map(DiagnosisContract c) => new()
    {
        Id = c.DiagnosisContractId,
        ContractCode = c.ContractCode,
        ExpertId = c.ExpertId,
        ExpertName = c.Expert?.Fullname ?? "",
        BankAccount = c.BankAccount,
        BankName = c.BankName,
        BankBin = c.BankBin,
        AccountHolder = c.AccountHolder,
        PricePerDiagnosis = c.PricePerDiagnosis,
        StartDate = c.StartDate,
        EndDate = c.EndDate,
        Status = c.Status,
        Notes = c.Notes,
        CreatedAt = c.CreatedAt
    };

    private static ApiResponse<ContractResponse> Fail(string message)
        => new() { Success = false, Message = message };

    private static bool CanAccess(DiagnosisContract c, Guid userId, string role) => role switch
    {
        "Owner" => c.CreatedBy == userId,
        "Specialist" => c.ExpertId == userId,
        _ => false
    };
}
