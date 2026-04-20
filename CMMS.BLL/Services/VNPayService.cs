using System.Net;
using System.Security.Cryptography;
using System.Text;
using CMMS.BLL.Configuration;
using CMMS.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace CMMS.BLL.Services;

public class VNPayService : IPaymentGateway
{
    private readonly VNPaySettings _settings;

    public string ProviderName => "vnpay";

    public VNPayService(IOptions<VNPaySettings> settings)
    {
        _settings = settings.Value;
    }

    public Task<CreatePaymentResult> CreatePaymentUrlAsync(Guid paymentId, decimal amount, string orderInfo)
    {
        var vnp = new SortedDictionary<string, string>
        {
            { "vnp_Version", _settings.Version },
            { "vnp_Command", "pay" },
            { "vnp_TmnCode", _settings.TmnCode },
            { "vnp_Amount", ((long)(amount * 100)).ToString() },
            { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") },
            { "vnp_CurrCode", "VND" },
            { "vnp_IpAddr", "127.0.0.1" },
            { "vnp_Locale", "vn" },
            { "vnp_OrderInfo", orderInfo },
            { "vnp_OrderType", "other" },
            { "vnp_ReturnUrl", _settings.ReturnUrl },
            { "vnp_TxnRef", paymentId.ToString() }
        };

        var signData = string.Join("&", vnp.Select(x =>
            $"{x.Key}={WebUtility.UrlEncode(x.Value)}"));
        var secureHash = HmacSHA512(_settings.HashSecret, signData);

        return Task.FromResult(new CreatePaymentResult
        {
            Url = $"{_settings.BaseUrl}?{signData}&vnp_SecureHash={secureHash}"
        });
    }

    public Task<PaymentCallbackResult> ProcessCallbackAsync(IQueryCollection query)
    {
        var vnpParams = new SortedDictionary<string, string>();
        foreach (var key in query.Keys.Where(k => k.StartsWith("vnp_")))
            vnpParams[key] = query[key].ToString();

        var secureHash = vnpParams.GetValueOrDefault("vnp_SecureHash", "");
        vnpParams.Remove("vnp_SecureHash");
        vnpParams.Remove("vnp_SecureHashType");

        var signData = string.Join("&", vnpParams.Select(x =>
            $"{x.Key}={WebUtility.UrlEncode(x.Value)}"));
        var checkHash = HmacSHA512(_settings.HashSecret, signData);

        var result = new PaymentCallbackResult
        {
            PaymentId = query["vnp_TxnRef"].ToString(),
            ResponseCode = query["vnp_ResponseCode"].ToString(),
            Success = checkHash.Equals(secureHash, StringComparison.OrdinalIgnoreCase)
                      && query["vnp_ResponseCode"].ToString() == "00"
        };

        foreach (var key in query.Keys)
            result.RawData[key] = query[key].ToString();

        return Task.FromResult(result);
    }

    private static string HmacSHA512(string key, string data)
    {
        using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }
}
