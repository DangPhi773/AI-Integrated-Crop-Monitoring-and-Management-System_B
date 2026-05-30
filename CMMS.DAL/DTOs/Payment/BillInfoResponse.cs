namespace CMMS.DAL.DTOs.Payment;

public class BillInfoResponse
{
    public Guid SpecialistId { get; set; }
    public string SpecialistName { get; set; } = null!;
    public DateTime Month { get; set; }

    public string? BankAccount { get; set; }
    public string? BankName { get; set; }
    public string? BankBin { get; set; }
    public string? AccountHolder { get; set; }

    public int TotalDiagnoses { get; set; }
    public decimal TotalAmount { get; set; }
    public bool IsPaid { get; set; }

    public string? QrUrl { get; set; }

    public List<BillItemResponse> Items { get; set; } = new();
}
