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

    public async Task<ApiResponse<BillInfoResponse>> GetBillForMonthAsync(Guid specialistId, DateTime month, Guid userId, string role)
    {
        if (!CanAccessSpecialist(specialistId, userId, role))
            return new ApiResponse<BillInfoResponse> { Success = false, Message = "Không có quyền truy cập" };

        var specialist = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == specialistId);
        if (specialist == null)
            return new ApiResponse<BillInfoResponse> { Success = false, Message = "Không tìm thấy chuyên gia" };

        var monthStart = new DateTime(month.Year, month.Month, 1);

        var items = await QueryUnpaidItemsAsync(specialistId, monthStart);

        var isPaid = await _db.DiagnosisPayments
            .AnyAsync(p => p.SpecialistId == specialistId && p.Month == monthStart);

        var latestContract = await _db.DiagnosisContracts
            .Where(c => c.ExpertId == specialistId)
            .OrderByDescending(c => c.StartDate)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return new ApiResponse<BillInfoResponse>
        {
            Success = true,
            Data = new BillInfoResponse
            {
                SpecialistId = specialistId,
                SpecialistName = specialist.Fullname ?? "",
                Month = monthStart,
                BankAccount = latestContract?.BankAccount,
                BankName = latestContract?.BankName,
                AccountHolder = latestContract?.AccountHolder,
                TotalDiagnoses = items.Count,
                TotalAmount = items.Sum(i => i.UnitPrice),
                IsPaid = isPaid,
                Items = items
            }
        };
    }

    public async Task<ApiResponse<PaymentResponse>> UploadPaymentAsync(UploadPaymentRequest request, Guid ownerId)
    {
        var (ok, error) = FileValidator.ValidateImage(request.BillFile, maxMb: 5);
        if (!ok)
            return new ApiResponse<PaymentResponse> { Success = false, Message = error! };

        var monthStart = new DateTime(request.Month.Year, request.Month.Month, 1);

        if (monthStart.AddMonths(1) > DateTimeHelper.VnNow().Date)
            return new ApiResponse<PaymentResponse> { Success = false, Message = "Chỉ thanh toán cho tháng đã kết thúc" };

        var specialist = await _db.Users.FirstOrDefaultAsync(u => u.UserId == request.SpecialistId);
        if (specialist == null)
            return new ApiResponse<PaymentResponse> { Success = false, Message = "Không tìm thấy chuyên gia" };

        var alreadyPaid = await _db.DiagnosisPayments
            .AnyAsync(p => p.SpecialistId == request.SpecialistId && p.Month == monthStart);
        if (alreadyPaid)
            return new ApiResponse<PaymentResponse> { Success = false, Message = "Tháng này đã thanh toán" };

        var items = await QueryUnpaidItemsAsync(request.SpecialistId, monthStart);
        if (items.Count == 0)
            return new ApiResponse<PaymentResponse> { Success = false, Message = "Không có chẩn đoán nào để thanh toán" };

        var totalAmount = items.Sum(i => i.UnitPrice);
        var upload = await _cloudinary.UploadFileAsync(request.BillFile, "payment-bills");
        var now = DateTimeHelper.VnNow();

        var payment = new DiagnosisPayment
        {
            DiagnosisPaymentId = Guid.NewGuid(),
            SpecialistId = request.SpecialistId,
            Month = monthStart,
            TotalDiagnoses = items.Count,
            Amount = totalAmount,
            Status = "paid",
            BillImageUrl = upload.SecureUrl,
            BillPublicId = upload.PublicId,
            CreatedAt = now,
            PaidAt = now
        };
        _db.DiagnosisPayments.Add(payment);

        foreach (var it in items)
        {
            _db.DiagnosisPaymentItems.Add(new DiagnosisPaymentItem
            {
                DiagnosisPaymentItemId = Guid.NewGuid(),
                PaymentId = payment.DiagnosisPaymentId,
                DiagnosisResultId = it.DiagnosisResultId,
                ContractId = it.ContractId
            });
        }

        await _db.SaveChangesAsync();

        await _realtime.PushPaymentStatusAsync(request.SpecialistId, new
        {
            paymentId = payment.DiagnosisPaymentId,
            specialistId = request.SpecialistId,
            month = monthStart,
            totalDiagnoses = items.Count,
            amount = totalAmount,
            billImageUrl = upload.SecureUrl,
            paidAt = now
        });

        var response = await BuildPaymentResponse(payment.DiagnosisPaymentId);
        return new ApiResponse<PaymentResponse>
        {
            Success = true,
            Data = response,
            Message = "Thanh toán đã được ghi nhận"
        };
    }

    public async Task<ApiResponse<IEnumerable<PaymentResponse>>> GetMyPaymentsAsync(Guid userId, string role)
    {
        var query = _db.DiagnosisPayments
            .Include(p => p.Specialist)
            .Include(p => p.Items)
            .AsNoTracking()
            .OrderByDescending(p => p.PaidAt ?? p.CreatedAt)
            .AsQueryable();

        query = role switch
        {
            "Owner" => query,
            "Specialist" => query.Where(p => p.SpecialistId == userId),
            _ => query.Where(p => false)
        };

        var payments = await query.ToListAsync();
        var responses = new List<PaymentResponse>();
        foreach (var p in payments)
            responses.Add(await MapPaymentAsync(p));

        return new ApiResponse<IEnumerable<PaymentResponse>>
        {
            Success = true,
            Data = responses
        };
    }

    public async Task<ApiResponse<PaymentResponse>> GetPaymentByIdAsync(Guid paymentId, Guid userId, string role)
    {
        var p = await _db.DiagnosisPayments
            .Include(x => x.Specialist)
            .Include(x => x.Items)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.DiagnosisPaymentId == paymentId);

        if (p == null || !CanAccessSpecialist(p.SpecialistId, userId, role))
            return new ApiResponse<PaymentResponse> { Success = false, Message = "Không tìm thấy thanh toán" };

        return new ApiResponse<PaymentResponse> { Success = true, Data = await MapPaymentAsync(p) };
    }

    private async Task<List<BillItemResponse>> QueryUnpaidItemsAsync(Guid specialistId, DateTime monthStart)
    {
        var monthEnd = monthStart.AddMonths(1);

        var raw = await (
            from dr in _db.DiagnosisResults.AsNoTracking()
            join c in _db.DiagnosisContracts.AsNoTracking()
                on dr.DiagnosedBy equals c.ExpertId
            join r in _db.Reports.AsNoTracking()
                on dr.ReportId equals r.ReportId
            join s in _db.Seasons.AsNoTracking()
                on r.SeasonId equals s.SeasonId into sJoin
            from s in sJoin.DefaultIfEmpty()
            join f in _db.Farms.AsNoTracking()
                on (s != null ? s.FarmId : Guid.Empty) equals f.FarmId into fJoin
            from f in fJoin.DefaultIfEmpty()
            where dr.DiagnosedBy == specialistId
                && dr.Status == "FINAL"
                && dr.CreatedAt >= monthStart
                && dr.CreatedAt < monthEnd
                && dr.CreatedAt >= c.StartDate
                && (c.EndDate == null || dr.CreatedAt <= c.EndDate)
                && !_db.DiagnosisPaymentItems.Any(pi => pi.DiagnosisResultId == dr.DiagnosisResultId)
            select new BillItemResponse
            {
                DiagnosisResultId = dr.DiagnosisResultId,
                DiseaseName = dr.DiseaseName,
                DiagnosedAt = dr.CreatedAt,
                ReportId = dr.ReportId,
                ReportNo = r.ReportNo,
                ContractId = c.DiagnosisContractId,
                ContractCode = c.ContractCode,
                UnitPrice = c.PricePerDiagnosis,
                FarmId = f != null ? f.FarmId : (Guid?)null,
                FarmName = f != null ? f.FarmName : null
            }
        ).ToListAsync();

        return raw;
    }

    private async Task<PaymentResponse> BuildPaymentResponse(Guid paymentId)
    {
        var p = await _db.DiagnosisPayments
            .Include(x => x.Specialist)
            .Include(x => x.Items)
            .AsNoTracking()
            .FirstAsync(x => x.DiagnosisPaymentId == paymentId);
        return await MapPaymentAsync(p);
    }

    private async Task<PaymentResponse> MapPaymentAsync(DiagnosisPayment p)
    {
        var resultIds = p.Items.Select(i => i.DiagnosisResultId).ToList();
        var contractIds = p.Items.Select(i => i.ContractId).Distinct().ToList();

        var diagnoses = await _db.DiagnosisResults.AsNoTracking()
            .Where(dr => resultIds.Contains(dr.DiagnosisResultId))
            .Select(dr => new { dr.DiagnosisResultId, dr.DiseaseName, dr.CreatedAt, dr.ReportId })
            .ToDictionaryAsync(x => x.DiagnosisResultId);

        var contracts = await _db.DiagnosisContracts.AsNoTracking()
            .Where(c => contractIds.Contains(c.DiagnosisContractId))
            .Select(c => new { c.DiagnosisContractId, c.ContractCode, c.PricePerDiagnosis })
            .ToDictionaryAsync(x => x.DiagnosisContractId);

        var reportIds = diagnoses.Values.Select(d => d.ReportId).Distinct().ToList();
        var reportInfo = await (
            from r in _db.Reports.AsNoTracking()
            join s in _db.Seasons.AsNoTracking() on r.SeasonId equals s.SeasonId into sj
            from s in sj.DefaultIfEmpty()
            join f in _db.Farms.AsNoTracking() on (s != null ? s.FarmId : Guid.Empty) equals f.FarmId into fj
            from f in fj.DefaultIfEmpty()
            where reportIds.Contains(r.ReportId)
            select new
            {
                r.ReportId,
                r.ReportNo,
                FarmId = f != null ? (Guid?)f.FarmId : null,
                FarmName = f != null ? f.FarmName : null
            }
        ).ToDictionaryAsync(x => x.ReportId);

        var items = p.Items.Select(it =>
        {
            diagnoses.TryGetValue(it.DiagnosisResultId, out var dr);
            contracts.TryGetValue(it.ContractId, out var ct);
            Guid? farmId = null;
            string? farmName = null;
            string? reportNo = null;
            if (dr != null && reportInfo.TryGetValue(dr.ReportId, out var info))
            {
                farmId = info.FarmId;
                farmName = info.FarmName;
                reportNo = info.ReportNo;
            }
            return new PaymentItemResponse
            {
                Id = it.DiagnosisPaymentItemId,
                DiagnosisResultId = it.DiagnosisResultId,
                DiseaseName = dr?.DiseaseName ?? "",
                DiagnosedAt = dr?.CreatedAt ?? default,
                ReportId = dr?.ReportId ?? Guid.Empty,
                ReportNo = reportNo,
                ContractId = it.ContractId,
                ContractCode = ct?.ContractCode ?? "",
                UnitPrice = ct?.PricePerDiagnosis ?? 0,
                FarmId = farmId,
                FarmName = farmName
            };
        }).ToList();

        return new PaymentResponse
        {
            Id = p.DiagnosisPaymentId,
            SpecialistId = p.SpecialistId,
            SpecialistName = p.Specialist?.Fullname ?? "",
            Month = p.Month,
            TotalDiagnoses = p.TotalDiagnoses,
            Amount = p.Amount,
            BillImageUrl = p.BillImageUrl,
            Status = p.Status,
            CreatedAt = p.CreatedAt,
            PaidAt = p.PaidAt,
            Items = items
        };
    }

    private static bool CanAccessSpecialist(Guid specialistId, Guid userId, string role) => role switch
    {
        "Owner" => true,
        "Specialist" => specialistId == userId,
        _ => false
    };
}
