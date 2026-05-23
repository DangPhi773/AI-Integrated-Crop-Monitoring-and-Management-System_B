using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMMS.DAL.DTOs.Tasks
{
    namespace CMMS.DAL.DTOs.Tasks
    {
        public class RecommendationTaskDetailRequest
        {
            public Guid? TaskId { get; set; }
            public Guid? SeasonId { get; set; }
            public Guid? FarmId { get; set; }

            [StringLength(200, MinimumLength = 1, ErrorMessage = "Title phải từ 1 đến 200 ký tự")]
            public string? Title { get; set; }

            [Range(0.0, 1000000, ErrorMessage = "Quantity phải từ 0 đến 1,000,000")]
            public decimal? Quantity { get; set; }

            [StringLength(50, ErrorMessage = "Status tối đa 50 ký tự")]
            public string? Status { get; set; }

            [StringLength(50, ErrorMessage = "Unit tối đa 50 ký tự")]
            public string? Unit { get; set; }

            [StringLength(1000, ErrorMessage = "Notes tối đa 1000 ký tự")]
            public string? Notes { get; set; }

            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }

            public List<Guid> AssignedToWorkerIds { get; set; } = new();
            public List<Guid> PlotIds { get; set; } = new();
            public List<Guid> BedIds { get; set; } = new();
        }

        public class RecommendationTaskDetailResponse
        {
            public Guid TaskDetailId { get; set; }
            public Guid? TaskId { get; set; }
            public string? TaskTitle { get; set; }
            public Guid? SeasonId { get; set; }
            public Guid? FarmId { get; set; }
            public string? Title { get; set; }
            public decimal? Quantity { get; set; }
            public string? Status { get; set; }
            public string? Unit { get; set; }
            public string? Notes { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }

            public List<Guid> AssignedToWorkerIds { get; set; } = new();
            public List<Guid> PlotIds { get; set; } = new();
            public List<Guid> BedIds { get; set; } = new();

            public List<string>? WorkerNames { get; set; }
        }
    }
}
