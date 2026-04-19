using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.DTOs.Tasks
{
    public class RecommendationTaskRequest
    {
        public Guid? CreatedByOwnerId { get; set; }
        public Guid? AssignedToWorkerId { get; set; }
        public string? Title { get; set; }
        public DateTime? TaskScheduledAt { get; set; }
        public string? TaskStatus { get; set; }
    }

    public class RecommendationTaskResponse
    {
        public Guid RecommendationTaskId { get; set; }
        public string? Title { get; set; }
        public string? TaskStatus { get; set; }
        public DateTime? TaskScheduledAt { get; set; }
        public DateTime? TaskCreatedAt { get; set; }
        public string? OwnerName { get; set; }
        public string? WorkerName { get; set; }
        public int TotalDetails { get; set; }
    }
}
