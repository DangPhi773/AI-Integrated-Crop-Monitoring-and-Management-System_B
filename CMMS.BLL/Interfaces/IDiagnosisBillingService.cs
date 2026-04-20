using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Payment;
using Microsoft.AspNetCore.Http;

namespace CMMS.BLL.Interfaces;

public interface IDiagnosisBillingService
{
    Task<ApiResponse<BillInfoResponse>> CreatePriceSettingAsync(CreatePriceSettingRequest request, Guid userId);
    Task<ApiResponse<BillInfoResponse>> GetBillInfoAsync(Guid priceSettingId);
    Task<ApiResponse<BillInfoResponse>> GetBillInfoByParamsAsync(Guid farmId, Guid expertId, DateTime month);
    Task<ApiResponse<PaymentUrlResponse>> CreatePaymentAsync(CreatePaymentRequest request);
    Task<ApiResponse<string>> ProcessPaymentCallbackAsync(string provider, IQueryCollection query);
    Task<ApiResponse<IEnumerable<BillInfoResponse>>> GetAllPriceSettingsAsync();
    Task<ApiResponse<IEnumerable<BillInfoResponse>>> GetMyBillsAsync(Guid expertId);
}
