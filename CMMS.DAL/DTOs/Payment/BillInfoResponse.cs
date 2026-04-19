namespace CMMS.DAL.DTOs.Payment;

public class BillInfoResponse
{
    public Guid PriceSettingId { get; set; }
    public string FarmName { get; set; } = null!;
    public string ExpertName { get; set; } = null!;
    public DateTime Month { get; set; }
    public decimal PricePerDiagnosis { get; set; }
    public int TotalDiagnoses { get; set; }
    public decimal TotalAmount { get; set; }
    public bool IsPaid { get; set; }
}
