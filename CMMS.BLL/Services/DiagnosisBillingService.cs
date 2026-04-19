using CMMS.BLL.Helpers;
using CMMS.BLL.Interfaces;
using CMMS.DAL.DBContext;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Payment;
using CMMS.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CMMS.BLL.Services;

public class DiagnosisBillingService : IDiagnosisBillingService
{
    private readonly AppDbContext _db;
    private readonly VNPayService _vnpay;
    private readonly PayOSService _payos;

    public DiagnosisBillingService(AppDbContext db, VNPayService vnpay, PayOSService payos)
    {
        _db = db;
        _vnpay = vnpay;
        _payos = payos;
    }

    public async Task<ApiResponse<BillInfoResponse>> CreatePriceSettingAsync(CreatePriceSettingRequest request, Guid userId)
    {
        var month = new DateTime(request.Month.Year, request.Month.Month, 1);

        var existing = await _db.DiagnosisPriceSettings
            .FirstOrDefaultAsync(x => x.FarmId == request.FarmId
                && x.ExpertId == request.ExpertId
                && x.Month == month);

        if (existing != null)
        {
            existing.PricePerDiagnosis = request.PricePerDiagnosis;
            existing.Notes = request.Notes;
        }
        else
        {
            existing = new DiagnosisPriceSetting
            {
                Id = Guid.NewGuid(),
                FarmId = request.FarmId,
                ExpertId = request.ExpertId,
                Month = month,
                PricePerDiagnosis = request.PricePerDiagnosis,
                Notes = request.Notes,
                CreatedBy = userId,
                CreatedAt = DateTimeHelper.VnNow()
            };
            _db.DiagnosisPriceSettings.Add(existing);
        }

        await _db.SaveChangesAsync();

        var bill = await BuildBillInfo(existing.Id);
        return new ApiResponse<BillInfoResponse> { Success = true, Data = bill };
    }

    public async Task<ApiResponse<BillInfoResponse>> GetBillInfoAsync(Guid priceSettingId)
    {
        var bill = await BuildBillInfo(priceSettingId);
        if (bill == null)
            return new ApiResponse<BillInfoResponse> { Success = false, Message = "Không tìm thấy cài đặt giá" };
        return new ApiResponse<BillInfoResponse> { Success = true, Data = bill };
    }

    public async Task<ApiResponse<BillInfoResponse>> GetBillInfoByParamsAsync(Guid farmId, Guid expertId, DateTime month)
    {
        var normalizedMonth = new DateTime(month.Year, month.Month, 1);

        var setting = await _db.DiagnosisPriceSettings
            .FirstOrDefaultAsync(x => x.FarmId == farmId
                && x.ExpertId == expertId
                && x.Month == normalizedMonth);

        if (setting == null)
            return new ApiResponse<BillInfoResponse> { Success = false, Message = "Không tìm thấy cài đặt giá cho tháng này" };

        var bill = await BuildBillInfo(setting.Id);
        return new ApiResponse<BillInfoResponse> { Success = true, Data = bill };
    }

    public async Task<ApiResponse<PaymentUrlResponse>> CreatePaymentAsync(CreatePaymentRequest request)
    {
        var setting = await _db.DiagnosisPriceSettings
            .Include(x => x.Farm)
            .Include(x => x.Expert)
            .FirstOrDefaultAsync(x => x.Id == request.PriceSettingId);

        if (setting == null)
            return new ApiResponse<PaymentUrlResponse> { Success = false, Message = "Không tìm thấy cài đặt giá" };

        var isPaid = await _db.DiagnosisPayments
            .AnyAsync(p => p.PriceSettingId == setting.Id && p.Status == "success");
        if (isPaid)
            return new ApiResponse<PaymentUrlResponse> { Success = false, Message = "Hóa đơn này đã thanh toán" };

        var totalDiagnoses = await CountDiagnoses(setting);
        if (totalDiagnoses == 0)
            return new ApiResponse<PaymentUrlResponse> { Success = false, Message = "Không có chẩn đoán nào trong tháng" };

        var totalAmount = totalDiagnoses * setting.PricePerDiagnosis;

        var payment = new DiagnosisPayment
        {
            Id = Guid.NewGuid(),
            PriceSettingId = request.PriceSettingId,
            TotalDiagnoses = totalDiagnoses,
            Amount = totalAmount,
            Status = "pending",
            PaymentProvider = request.Provider.ToLower(),
            CreatedAt = DateTimeHelper.VnNow()
        };
        _db.DiagnosisPayments.Add(payment);
        await _db.SaveChangesAsync();

        var orderInfo = $"Thanh toan chan doan {setting.Month:MM/yyyy} - {setting.Expert?.Fullname}";
        var gateway = GetGateway(request.Provider);
        var paymentUrl = gateway.CreatePaymentUrl(payment.Id, totalAmount, orderInfo);

        return new ApiResponse<PaymentUrlResponse>
        {
            Success = true,
            Data = new PaymentUrlResponse { PaymentId = payment.Id, PaymentUrl = paymentUrl }
        };
    }

    public async Task<ApiResponse<string>> ProcessPaymentCallbackAsync(string provider, IQueryCollection query)
    {
        var gateway = GetGateway(provider);
        var result = gateway.ProcessCallback(query);

        if (!Guid.TryParse(result.PaymentId, out var paymentId))
            return new ApiResponse<string> { Success = false, Message = "PaymentId không hợp lệ" };

        var payment = await _db.DiagnosisPayments.FirstOrDefaultAsync(p => p.Id == paymentId);
        if (payment == null)
            return new ApiResponse<string> { Success = false, Message = "Không tìm thấy giao dịch" };

        payment.Status = result.Success ? "success" : "failed";
        payment.ProviderData = JsonSerializer.Serialize(result.RawData);

        if (result.Success)
            payment.PaidAt = DateTimeHelper.VnNow();

        await _db.SaveChangesAsync();

        return new ApiResponse<string>
        {
            Success = result.Success,
            Message = result.Success ? "Thanh toán thành công" : "Thanh toán thất bại"
        };
    }

    private async Task<BillInfoResponse?> BuildBillInfo(Guid priceSettingId)
    {
        var setting = await _db.DiagnosisPriceSettings
            .Include(x => x.Farm)
            .Include(x => x.Expert)
            .FirstOrDefaultAsync(x => x.Id == priceSettingId);

        if (setting == null) return null;

        var totalDiagnoses = await CountDiagnoses(setting);

        var isPaid = await _db.DiagnosisPayments
            .AnyAsync(p => p.PriceSettingId == setting.Id && p.Status == "success");

        return new BillInfoResponse
        {
            PriceSettingId = setting.Id,
            FarmName = setting.Farm?.FarmName ?? "",
            ExpertName = setting.Expert?.Fullname ?? "",
            Month = setting.Month,
            PricePerDiagnosis = setting.PricePerDiagnosis,
            TotalDiagnoses = totalDiagnoses,
            TotalAmount = totalDiagnoses * setting.PricePerDiagnosis,
            IsPaid = isPaid
        };
    }

    private async Task<int> CountDiagnoses(DiagnosisPriceSetting setting)
    {
        var monthStart = setting.Month;
        var monthEnd = monthStart.AddMonths(1);

        return await _db.DiagnosisResults
            .Where(dr => dr.DiagnosedBy == setting.ExpertId
                && dr.Status == "confirmed"
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
            .Where(x => x.s.FarmId == setting.FarmId)
            .CountAsync();
    }

    private IPaymentGateway GetGateway(string provider)
    {
        return provider.ToLower() switch
        {
            "vnpay" => _vnpay,
            "payos" => _payos,
            _ => throw new ArgumentException($"Provider không hỗ trợ: {provider}")
        };
    }

    public async Task<ApiResponse<IEnumerable<BillInfoResponse>>> GetAllPriceSettingsAsync()
    {
        try
        {
            var settingsIds = await _db.DiagnosisPriceSettings
                .AsNoTracking()
                .OrderByDescending(x => x.Month)
                .Select(x => x.Id)
                .ToListAsync();

            var resultList = new List<BillInfoResponse>();

            foreach (var id in settingsIds)
            {
                var bill = await BuildBillInfo(id);
                if (bill != null) resultList.Add(bill);
            }

            return new ApiResponse<IEnumerable<BillInfoResponse>>
            {
                Success = true,
                Data = resultList,
                Message = $"Lấy được {resultList.Count} bản ghi cấu hình giá."
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<BillInfoResponse>>
            {
                Success = false,
                Message = "Lỗi khi lấy danh sách cấu hình giá",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}
