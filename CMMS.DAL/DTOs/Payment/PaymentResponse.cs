namespace CMMS.DAL.DTOs.Payment;

public class PaymentResponse
{
    public Guid Id { get; set; }
    public Guid SpecialistId { get; set; }
    public string SpecialistName { get; set; } = null!;
    public DateTime Month { get; set; }
    public int TotalDiagnoses { get; set; }
    public decimal Amount { get; set; }
    public string? BillImageUrl { get; set; }
    public string Status { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? PaidAt { get; set; }

    public List<PaymentItemResponse> Items { get; set; } = new();
}
