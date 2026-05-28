using System.ComponentModel.DataAnnotations;

namespace CMMS.DAL.DTOs.Reports.Requests
{
    public class AssignReportRequest
    {
        [Required(ErrorMessage = "AssignedTo là bắt buộc")]
        public Guid AssignedTo { get; set; }

        [StringLength(1000, ErrorMessage = "Note tối đa 1000 ký tự")]
        public string? Note { get; set; }
    }
}
