namespace CMMS.DAL.DTOs.Payment;

public class BillItemResponse
{
    public Guid DiagnosisResultId { get; set; }
    public string DiseaseName { get; set; } = null!;
    public DateTime DiagnosedAt { get; set; }
    public Guid ReportId { get; set; }
    public string? ReportNo { get; set; }
    public Guid ContractId { get; set; }
    public string ContractCode { get; set; } = null!;
    public decimal UnitPrice { get; set; }
    public Guid? FarmId { get; set; }
    public string? FarmName { get; set; }
}
