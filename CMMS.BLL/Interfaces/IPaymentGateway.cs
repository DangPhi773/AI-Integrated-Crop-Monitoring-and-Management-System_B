using Microsoft.AspNetCore.Http;

namespace CMMS.BLL.Interfaces;

public interface IPaymentGateway
{
    string ProviderName { get; }
    Task<CreatePaymentResult> CreatePaymentUrlAsync(Guid paymentId, decimal amount, string orderInfo);
    Task<PaymentCallbackResult> ProcessCallbackAsync(IQueryCollection query);
}

public class CreatePaymentResult
{
    public string Url { get; set; } = null!;
    public long? OrderCode { get; set; }
}

public class PaymentCallbackResult
{
    public bool Success { get; set; }
    public string PaymentId { get; set; } = null!;
    public string? ResponseCode { get; set; }
    public Dictionary<string, string> RawData { get; set; } = new();
}
