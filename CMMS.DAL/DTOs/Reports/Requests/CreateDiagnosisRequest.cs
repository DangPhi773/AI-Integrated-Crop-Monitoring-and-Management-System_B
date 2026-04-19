namespace CMMS.DAL.DTOs.Reports.Requests
{
    public class CreateDiagnosisRequest
    {
        public string DiseaseName { get; set; } = null!;
        public string Conclusion { get; set; } = null!;
        public string RecommendedAction { get; set; } = null!;
        public string? SeverityLevel { get; set; }
    }
}
