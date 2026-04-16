namespace CMMS.DAL.DTOs.Reports.Requests
{
    public class CreateReportRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ReportType { get; set; }
        public Guid? PlotId { get; set; }
        public Guid? BedId { get; set; }
        public Guid? SeasonId { get; set; }
        public Guid? OwnerId { get; set; }
        public string? AiResultsJson { get; set; }
    }
}
