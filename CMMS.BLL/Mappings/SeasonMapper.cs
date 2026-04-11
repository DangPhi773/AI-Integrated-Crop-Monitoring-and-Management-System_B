using CMMS.DAL.DTOs.Seasons;
using CMMS.DAL.DTOs.SeasonsDetails;
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
            SeasonsDetailsCount = s.SeasonsDetails?.Count ?? 0,
            TasksCount = s.TaskDetails?.Count ?? 0,
            SeasonsDetails = s.SeasonsDetails?.Select(d => new SeasonsDetailDto
            {
                SeasonDetailId = d.SeasonDetailId,
                BedId = d.BedId,
                CropId = d.CropId,
                CropQuantity = d.CropQuantity,
                StartDate = d.StartDate,
                EndDate = d.EndDate,
                SeasonExpectedHarvestDate = d.SeasonExpectedHarvestDate,
                TotalHarvestYield = d.TotalHarvestYield
            }).ToList() ?? new List<SeasonsDetailDto>()
        };
    }
}
