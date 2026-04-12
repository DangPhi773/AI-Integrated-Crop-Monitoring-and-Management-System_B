using CMMS.DAL.DTOs.Tasks;
using CMMS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Mappings
{
    public static class RecommendationTaskDetailMapper
    {
        public static RecommendationTaskDetailResponse ToResponse(RecommendationTaskDetail d) => new()
        {
            TaskDetailId = d.TaskDetailId,
            TaskId = d.TaskId,
            TaskTitle = d.Task?.Title,
            WorkerId = d.WorkerId,
            WorkerName = d.Worker?.Fullname, 
            Title = d.Title,
            Quantity = d.Quantity,
            Status = d.Status,
            Unit = d.Unit,
            Notes = d.Notes,
            StartDate = d.StartDate,
            EndDate = d.EndDate
        };

        public static IEnumerable<RecommendationTaskDetailResponse> ToResponseList(IEnumerable<RecommendationTaskDetail> list)
            => list.Select(ToResponse);
    }
}
