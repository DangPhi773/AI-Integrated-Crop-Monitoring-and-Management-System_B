using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMMS.DAL.DTOs.Tasks
{
    public class TaskRequest
    {
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Tiêu đề công việc phải từ 1 đến 200 ký tự")]
        public string? TaskTitle { get; set; }

        [StringLength(50, ErrorMessage = "TaskStatus tối đa 50 ký tự")]
        public string? TaskStatus { get; set; }

        [StringLength(1000, ErrorMessage = "TaskNotes tối đa 1000 ký tự")]
        public string? TaskNotes { get; set; }

        [StringLength(50, ErrorMessage = "TaskType tối đa 50 ký tự")]
        public string? TaskType { get; set; }
    }

    public class TaskResponse
    {
        public Guid TaskId { get; set; }
        public string? TaskTitle { get; set; }
        public string? TaskStatus { get; set; }
        public string? TaskNotes { get; set; }
        public string? TaskType { get; set; }
        public DateTime? TaskCreatedAt { get; set; }
        public DateTime? TaskScheduledAt { get; set; }
        public int TaskDetailsCount { get; set; }
    }

    public class TaskDetailRequest
    {
        public Guid? TaskId { get; set; }
        public Guid? SeasonId { get; set; }
        public Guid? FarmId { get; set; }
        public List<Guid>? AssignedToWorkerIds { get; set; }
        public List<Guid>? BedIds { get; set; }
        public List<Guid>? PlotIds { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [StringLength(1000, ErrorMessage = "Notes tối đa 1000 ký tự")]
        public string? Notes { get; set; }

        [StringLength(50, ErrorMessage = "Status tối đa 50 ký tự")]
        public string? Status { get; set; }
    }

    public class TaskDetailResponse
    {
        public Guid TaskDetailId { get; set; }
        public Guid? TaskId { get; set; }
        public string? TaskTitle { get; set; }
        public Guid? SeasonId { get; set; }
        public Guid? FarmId { get; set; }
        public List<Guid> AssignedToWorkerIds { get; set; } = new();
        public List<Guid> BedIds { get; set; } = new();
        public List<Guid> PlotIds { get; set; } = new();
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Notes { get; set; }
        public string? Status { get; set; }
    }

    public class UpdateTaskDetailStatusRequest
    {
        [Required(ErrorMessage = "Status là bắt buộc")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Status phải từ 1 đến 50 ký tự")]
        public string Status { get; set; } = null!;
    }
}
