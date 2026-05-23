using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Payment;

namespace CMMS.BLL.Interfaces;

public interface IDiagnosisBillingService
{
    Task<ApiResponse<BillInfoResponse>> GetBillForMonthAsync(Guid specialistId, DateTime month, Guid userId, string role);
    Task<ApiResponse<PaymentResponse>> UploadPaymentAsync(UploadPaymentRequest request, Guid ownerId);
    Task<ApiResponse<IEnumerable<PaymentResponse>>> GetMyPaymentsAsync(Guid userId, string role);
    Task<ApiResponse<PaymentResponse>> GetPaymentByIdAsync(Guid paymentId, Guid userId, string role);
}
