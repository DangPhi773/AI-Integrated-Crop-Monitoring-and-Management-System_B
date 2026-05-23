using System.ComponentModel.DataAnnotations;

namespace CMMS.DAL.DTOs.Reports.Requests
{
    public class CreateReportRequest
    {
        [Required(ErrorMessage = "Title là bắt buộc")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Title phải từ 1 đến 200 ký tự")]
        public string? Title { get; set; }

        [StringLength(2000, ErrorMessage = "Description tối đa 2000 ký tự")]
        public string? Description { get; set; }

        [StringLength(50, ErrorMessage = "ReportType tối đa 50 ký tự")]
        public string? ReportType { get; set; }

        public Guid? PlotId { get; set; }
        public Guid? BedId { get; set; }
        public Guid? SeasonId { get; set; }
        public Guid? OwnerId { get; set; }
        public string? AiResultsJson { get; set; }
    }
}
