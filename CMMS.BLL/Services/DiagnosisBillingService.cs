using CMMS.BLL.Helpers;
using CMMS.BLL.Interfaces;
using CMMS.BLL.Realtime;
using CMMS.DAL.DBContext;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Payment;
using CMMS.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CMMS.BLL.Services;

public class DiagnosisBillingService : IDiagnosisBillingService
{
    private readonly AppDbContext _db;
    private readonly ICloudinaryService _cloudinary;
    private readonly IPaymentRealtime _realtime;

    public DiagnosisBillingService(AppDbContext db, ICloudinaryService cloudinary, IPaymentRealtime realtime)
    {
        _db = db;
        _cloudinary = cloudinary;
        _realtime = realtime;
    }

    public async Task<ApiResponse<BillInfoResponse>> GetBillForMonthAsync(Guid contractId, DateTime month, Guid userId, string role)
    {
        var contract = await _db.DiagnosisContracts
            .Include(c => c.Farm)
            .Include(c => c.Expert)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == contractId);

        if (contract == null || !CanAccessContract(contract, userId, role))
            return new ApiResponse<BillInfoResponse> { Success = false, Message = "Không tìm thấy hợp đồng" };

        var monthStart = new DateTime(month.Year, month.Month, 1);
        var totalDiagnoses = await CountDiagnoses(contract.FarmId, contract.ExpertId, monthStart);

        var isPaid = await _db.DiagnosisPayments
            .AnyAsync(p => p.ContractId == contract.Id && p.Month == monthStart && p.Status == "paid");

        return new ApiResponse<BillInfoResponse>
        {
            Success = true,
            Data = new BillInfoResponse
            {
                ContractId = contract.Id,
                ContractCode = contract.ContractCode,
                FarmName = contract.Farm?.FarmName ?? "",
                ExpertName = contract.Expert?.Fullname ?? "",
                BankAccount = contract.BankAccount,
                BankName = contract.BankName,
                AccountHolder = contract.AccountHolder,
                Month = monthStart,
                PricePerDiagnosis = contract.PricePerDiagnosis,
                TotalDiagnoses = totalDiagnoses,
                TotalAmount = totalDiagnoses * contract.PricePerDiagnosis,
                IsPaid = isPaid
            }
        };
    }

    public async Task<ApiResponse<PaymentResponse>> UploadPaymentAsync(UploadPaymentRequest request, Guid ownerId)
    {
        var (ok, error) = FileValidator.ValidateImage(request.BillFile, maxMb: 5);
        if (!ok)
            return new ApiResponse<PaymentResponse> { Success = false, Message = error! };

        var contract = await _db.DiagnosisContracts
            .Include(c => c.Farm)
            .Include(c => c.Expert)
            .FirstOrDefaultAsync(c => c.Id == request.ContractId);

        if (contract == null || contract.CreatedBy != ownerId)
            return new ApiResponse<PaymentResponse> { Success = false, Message = "Không tìm thấy hợp đồng" };
        if (contract.Status != "active")
            return new ApiResponse<PaymentResponse> { Success = false, Message = "Hợp đồng đã kết thúc" };

        var monthStart = new DateTime(request.Month.Year, request.Month.Month, 1);

        var alreadyPaid = await _db.DiagnosisPayments
            .AnyAsync(p => p.ContractId == contract.Id && p.Month == monthStart && p.Status == "paid");
        if (alreadyPaid)
            return new ApiResponse<PaymentResponse> { Success = false, Message = "Tháng này đã thanh toán" };

        var totalDiagnoses = await CountDiagnoses(contract.FarmId, contract.ExpertId, monthStart);
        if (totalDiagnoses == 0)
            return new ApiResponse<PaymentResponse> { Success = false, Message = "Không có chẩn đoán nào trong tháng" };

        var totalAmount = totalDiagnoses * contract.PricePerDiagnosis;

        var upload = await _cloudinary.UploadFileAsync(request.BillFile, "payment-bills");

        var now = DateTimeHelper.VnNow();
        var payment = new DiagnosisPayment
        {
            Id = Guid.NewGuid(),
            ContractId = contract.Id,
            Month = monthStart,
            TotalDiagnoses = totalDiagnoses,
            Amount = totalAmount,
            Status = "paid",
            BillImageUrl = upload.SecureUrl,
            BillPublicId = upload.PublicId,
            CreatedAt = now,
            PaidAt = now
        };
        _db.DiagnosisPayments.Add(payment);
        await _db.SaveChangesAsync();

        await _realtime.PushPaymentStatusAsync(contract.ExpertId, new
        {
            paymentId = payment.Id,
            contractId = contract.Id,
            contractCode = contract.ContractCode,
            month = monthStart,
            amount = totalAmount,
            billImageUrl = upload.SecureUrl,
            paidAt = now
        });

        return new ApiResponse<PaymentResponse>
        {
            Success = true,
            Data = new PaymentResponse
            {
                Id = payment.Id,
                ContractId = contract.Id,
                ContractCode = contract.ContractCode,
                FarmName = contract.Farm?.FarmName ?? "",
                ExpertName = contract.Expert?.Fullname ?? "",
                Month = monthStart,
                TotalDiagnoses = totalDiagnoses,
                Amount = totalAmount,
                BillImageUrl = upload.SecureUrl,
                Status = "paid",
                CreatedAt = now,
                PaidAt = now
            },
            Message = "Thanh toán đã được ghi nhận"
        };
    }

    public async Task<ApiResponse<IEnumerable<PaymentResponse>>> GetMyPaymentsAsync(Guid userId, string role)
    {
        var query = _db.DiagnosisPayments
            .Include(p => p.Contract!).ThenInclude(c => c.Farm)
            .Include(p => p.Contract!).ThenInclude(c => c.Expert)
            .AsNoTracking()
            .OrderByDescending(p => p.PaidAt ?? p.CreatedAt)
            .AsQueryable();

        query = role switch
        {
            "Owner" => query.Where(p => p.Contract!.CreatedBy == userId),
            "Specialist" => query.Where(p => p.Contract!.ExpertId == userId),
            _ => query.Where(p => false)
        };

        var items = await query.ToListAsync();
        return new ApiResponse<IEnumerable<PaymentResponse>>
        {
            Success = true,
            Data = items.Select(Map)
        };
    }

    public async Task<ApiResponse<PaymentResponse>> GetPaymentByIdAsync(Guid paymentId, Guid userId, string role)
    {
        var p = await _db.DiagnosisPayments
            .Include(x => x.Contract!).ThenInclude(c => c.Farm)
            .Include(x => x.Contract!).ThenInclude(c => c.Expert)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == paymentId);

        if (p == null || p.Contract == null || !CanAccessContract(p.Contract, userId, role))
            return new ApiResponse<PaymentResponse> { Success = false, Message = "Không tìm thấy thanh toán" };

        return new ApiResponse<PaymentResponse> { Success = true, Data = Map(p) };
    }

    private static bool CanAccessContract(DiagnosisContract c, Guid userId, string role) => role switch
    {
        "Owner" => c.CreatedBy == userId,
        "Specialist" => c.ExpertId == userId,
        _ => false
    };

    private async Task<int> CountDiagnoses(Guid farmId, Guid expertId, DateTime monthStart)
    {
        var monthEnd = monthStart.AddMonths(1);

        return await _db.DiagnosisResults
            .Where(dr => dr.DiagnosedBy == expertId
                && dr.Status == "FINAL"
                && dr.CreatedAt >= monthStart
                && dr.CreatedAt < monthEnd)
            .Join(_db.Reports,
                dr => dr.ReportId,
                r => r.ReportId,
                (dr, r) => new { dr, r })
            .Join(_db.Seasons,
                x => x.r.SeasonId,
                s => s.SeasonId,
                (x, s) => new { x.dr, x.r, s })
            .Where(x => x.s.FarmId == farmId)
            .CountAsync();
    }

    private static PaymentResponse Map(DiagnosisPayment p) => new()
    {
        Id = p.Id,
        ContractId = p.ContractId,
        ContractCode = p.Contract?.ContractCode ?? "",
        FarmName = p.Contract?.Farm?.FarmName ?? "",
        ExpertName = p.Contract?.Expert?.Fullname ?? "",
        Month = p.Month,
        TotalDiagnoses = p.TotalDiagnoses,
        Amount = p.Amount,
        BillImageUrl = p.BillImageUrl,
        Status = p.Status,
        CreatedAt = p.CreatedAt,
        PaidAt = p.PaidAt
    };
}
