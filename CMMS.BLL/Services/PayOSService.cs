using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using CMMS.BLL.Configuration;
using CMMS.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace CMMS.BLL.Services;

public class PayOSService : IPaymentGateway
{
    private readonly PayOSSettings _settings;
    private readonly HttpClient _httpClient;

    public string ProviderName => "payos";

    public PayOSService(IOptions<PayOSSettings> settings, HttpClient httpClient)
    {
        _settings = settings.Value;
        _httpClient = httpClient;
    }

    public async Task<CreatePaymentResult> CreatePaymentUrlAsync(Guid paymentId, decimal amount, string orderInfo)
    {
        var orderCode = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var description = orderInfo.Length > 25 ? orderInfo[..25] : orderInfo;

        var signatureData = $"amount={(int)amount}&cancelUrl={_settings.CancelUrl}&description={description}&orderCode={orderCode}&returnUrl={_settings.ReturnUrl}";
        var signature = ComputeHmacSha256(_settings.ChecksumKey, signatureData);

        var body = new
        {
            orderCode,
            amount = (int)amount,
            description,
            returnUrl = _settings.ReturnUrl,
            cancelUrl = _settings.CancelUrl,
            signature
        };

        var request = new HttpRequestMessage(HttpMethod.Post, $"{_settings.BaseUrl}/v2/payment-requests");
        request.Headers.Add("x-client-id", _settings.ClientId);
        request.Headers.Add("x-api-key", _settings.ApiKey);
        request.Content = new StringContent(
            JsonSerializer.Serialize(body),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(responseContent);
        var checkoutUrl = doc.RootElement
            .GetProperty("data")
            .GetProperty("checkoutUrl")
            .GetString() ?? throw new Exception("PayOS không trả về checkoutUrl");

        return new CreatePaymentResult { Url = checkoutUrl, OrderCode = orderCode };
    }

    public async Task<PaymentCallbackResult> ProcessCallbackAsync(IQueryCollection query)
    {
        var result = new PaymentCallbackResult
        {
            PaymentId = query["orderCode"].ToString(),
            ResponseCode = query["status"].ToString(),
            Success = false
        };

        foreach (var key in query.Keys)
            result.RawData[key] = query[key].ToString();

        if (!long.TryParse(query["orderCode"].ToString(), out var orderCode))
            return result;

        var request = new HttpRequestMessage(HttpMethod.Get, $"{_settings.BaseUrl}/v2/payment-requests/{orderCode}");
        request.Headers.Add("x-client-id", _settings.ClientId);
        request.Headers.Add("x-api-key", _settings.ApiKey);

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode) return result;

        var responseContent = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(responseContent);

        if (!doc.RootElement.TryGetProperty("data", out var data)) return result;
        var status = data.TryGetProperty("status", out var s) ? s.GetString() : null;

        result.ResponseCode = status;
        result.Success = status == "PAID";
        result.RawData["server_verified_status"] = status ?? "";
        return result;
    }

    private static string ComputeHmacSha256(string key, string data)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }
}
