using CMMS.DAL.DTOs.Tasks;
using CMMS.DAL.DTOs.Tasks.CMMS.DAL.DTOs.Tasks;
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
            SeasonId = d.SeasonId,
            FarmId = d.FarmId,
            Title = d.Title,
            Quantity = d.Quantity,
            Status = d.Status,
            Unit = d.Unit,
            Notes = d.Notes,
            StartDate = d.StartDate,
            EndDate = d.EndDate,
            AssignedToWorkerIds = d.AssignedToWorkerIds ?? new List<Guid>(),
            PlotIds = d.PlotIds ?? new List<Guid>(),
            BedIds = d.BedIds ?? new List<Guid>(),
        };

        public static IEnumerable<RecommendationTaskDetailResponse> ToResponseList(IEnumerable<RecommendationTaskDetail> list)
            => list?.Select(ToResponse) ?? Enumerable.Empty<RecommendationTaskDetailResponse>();
        public static void MapToEntity(RecommendationTaskDetailRequest req, RecommendationTaskDetail entity)
        {
            entity.TaskId = req.TaskId;
            entity.SeasonId = req.SeasonId;
            entity.FarmId = req.FarmId;
            entity.Title = req.Title;
            entity.Quantity = req.Quantity;
            entity.Status = req.Status;
            entity.Unit = req.Unit;
            entity.Notes = req.Notes;
            entity.StartDate = req.StartDate;
            entity.EndDate = req.EndDate;
            entity.AssignedToWorkerIds = req.AssignedToWorkerIds;
            entity.PlotIds = req.PlotIds;
            entity.BedIds = req.BedIds;
        }
    }
}
