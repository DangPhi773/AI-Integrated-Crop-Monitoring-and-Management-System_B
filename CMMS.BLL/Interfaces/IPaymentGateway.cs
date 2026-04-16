using Microsoft.AspNetCore.Http;

namespace CMMS.BLL.Interfaces;

public interface IPaymentGateway
{
    string ProviderName { get; }
    string CreatePaymentUrl(Guid paymentId, decimal amount, string orderInfo);
    PaymentCallbackResult ProcessCallback(IQueryCollection query);
}

public class PaymentCallbackResult
{
    public bool Success { get; set; }
    public string PaymentId { get; set; } = null!;
    public string? ResponseCode { get; set; }
    public Dictionary<string, string> RawData { get; set; } = new();
}
