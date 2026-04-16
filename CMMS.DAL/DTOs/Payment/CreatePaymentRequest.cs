namespace CMMS.DAL.DTOs.Payment;

public class CreatePaymentRequest
{
    public Guid PriceSettingId { get; set; }
    public string Provider { get; set; } = "vnpay";
}
