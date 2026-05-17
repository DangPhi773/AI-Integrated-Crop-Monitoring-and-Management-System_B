namespace CMMS.DAL.DTOs.Payment;

public class ContractResponse
{
    public Guid Id { get; set; }
    public string ContractCode { get; set; } = null!;
    public Guid FarmId { get; set; }
    public string FarmName { get; set; } = null!;
    public Guid ExpertId { get; set; }
    public string ExpertName { get; set; } = null!;
    public string BankAccount { get; set; } = null!;
    public string BankName { get; set; } = null!;
    public string AccountHolder { get; set; } = null!;
    public decimal PricePerDiagnosis { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Status { get; set; } = null!;
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}
