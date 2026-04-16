namespace CMMS.DAL.DTOs.Payment;

public class PaymentUrlResponse
{
    public string PaymentUrl { get; set; } = null!;
    public Guid PaymentId { get; set; }
}
