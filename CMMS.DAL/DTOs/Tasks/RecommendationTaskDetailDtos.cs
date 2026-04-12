using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.DTOs.Tasks
{
    public class RecommendationTaskDetailRequest
    {
        public Guid? TaskId { get; set; }
        public Guid? WorkerId { get; set; }
        public string? Title { get; set; }
        public decimal? Quantity { get; set; }
        public string? Status { get; set; }
        public string? Unit { get; set; }
        public string? Notes { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class RecommendationTaskDetailResponse
    {
        public Guid TaskDetailId { get; set; }
        public Guid? TaskId { get; set; }
        public string? TaskTitle { get; set; } 
        public Guid? WorkerId { get; set; }
        public string? WorkerName { get; set; } 
        public string? Title { get; set; }
        public decimal? Quantity { get; set; }
        public string? Status { get; set; }
        public string? Unit { get; set; }
        public string? Notes { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
