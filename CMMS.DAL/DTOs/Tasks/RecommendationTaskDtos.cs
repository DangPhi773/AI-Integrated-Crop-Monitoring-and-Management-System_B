using System;
using System.ComponentModel.DataAnnotations;

namespace CMMS.DAL.DTOs.Tasks
{
    public class RecommendationTaskRequest
    {
        public Guid? CreatedByOwnerId { get; set; }
        public Guid? AssignedToWorkerId { get; set; }

        [StringLength(200, MinimumLength = 1, ErrorMessage = "Title phải từ 1 đến 200 ký tự")]
        public string? Title { get; set; }

        public DateTime? TaskScheduledAt { get; set; }

        [StringLength(50, ErrorMessage = "TaskStatus tối đa 50 ký tự")]
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
