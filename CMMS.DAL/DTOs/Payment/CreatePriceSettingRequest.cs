namespace CMMS.DAL.DTOs.Payment;

public class CreatePriceSettingRequest
{
    public Guid FarmId { get; set; }
    public Guid ExpertId { get; set; }
    public DateTime Month { get; set; }
    public decimal PricePerDiagnosis { get; set; }
    public string? Notes { get; set; }
}
