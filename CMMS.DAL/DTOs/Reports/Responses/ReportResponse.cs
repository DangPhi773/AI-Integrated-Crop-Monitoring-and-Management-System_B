using CMMS.DAL.DTOs.Attachments;

namespace CMMS.DAL.DTOs.Reports.Responses
{
    public class ReportResponse
    {
        public Guid ReportId { get; set; }
        public string? ReportNo { get; set; }
        public Guid? WorkerId { get; set; }
        public string? WorkerName { get; set; }
        public Guid? OwnerId { get; set; }
        public string? OwnerName { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ReportType { get; set; }
        public Guid? PlotId { get; set; }
        public Guid? BedId { get; set; }
        public Guid? SeasonId { get; set; }
        public string? AiResultsJson { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? SubmitDate { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<AttachmentDto> Attachments { get; set; } = new();
        public List<EnvironmentSnapshotDto> EnvironmentSnapshots { get; set; } = new();
    }
}
