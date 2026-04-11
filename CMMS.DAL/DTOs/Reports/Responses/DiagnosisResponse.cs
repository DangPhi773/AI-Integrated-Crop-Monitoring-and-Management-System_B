namespace CMMS.DAL.DTOs.Reports.Responses
{
    public class DiagnosisResponse
    {
        public Guid Id { get; set; }
        public Guid ReportId { get; set; }
        public string? ReportNo { get; set; }
        public string? ReportTitle { get; set; }
        public Guid DiagnosedBy { get; set; }
        public string? DiagnoserName { get; set; }
        public string DiseaseName { get; set; } = null!;
        public string Conclusion { get; set; } = null!;
        public string RecommendedAction { get; set; } = null!;
        public string? SeverityLevel { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
