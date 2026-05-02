using CMMS.DAL.DTOs.Seasons;
using CMMS.DAL.Entities;

namespace CMMS.BLL.Mappings
{
    public static class SeasonMapper
    {
        public static SeasonResponse ToResponse(Season s) => new()
        {
            SeasonId = s.SeasonId,
            FarmId = s.FarmId,
            SeasonName = s.SeasonName,
            SeasonStartDate = s.SeasonStartDate,
            SeasonEndDate = s.SeasonEndDate,
            Description = s.Description,
            SeasonNotes = s.SeasonNotes,
            SeasonCreatedAt = s.SeasonCreatedAt,
            Status = s.Status,
            HarvestsCount = s.Harvests?.Count ?? 0,
            TasksCount = s.TaskDetails?.Count ?? 0
        };
    }
}
