using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.DTOs.Reports
{
    public class ReportRequest
    {
        public Guid? WorkerId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public DateTime? SubmitDate { get; set; }
    }

    public class ReportResponse
    {
        public Guid ReportId { get; set; }
        public Guid? WorkerId { get; set; }
        public string? WorkerName { get; set; } // Lấy từ bảng User
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? SubmitDate { get; set; }
    }
}
