using CMMS.DAL.DTOs.Tasks;
using CMMS.DAL.Entities;

namespace CMMS.BLL.Mappings
{
    public static class CropGrowthTaskMapper
    {
        public static CropGrowthTaskResponse ToResponse(CropGrowthTask t) => new()
        {
            GrowthTaskId = t.GrowthTaskId,
            StageId = t.StageId,
            StageName = t.CropGrowthStage?.StageName,
            TaskId = t.TaskId,
            TaskTitle = t.Task?.TaskTitle,
            TaskDescription = t.TaskDescription,
            Frequency = t.Frequency,
            DurationMinutes = t.DurationMinutes,
            RequiredTools = t.RequiredTools,
            RequiredMaterials = t.RequiredMaterials,
            QuantityPerUnit = t.QuantityPerUnit,
            QuantityUnit = t.QuantityUnit,
            Priority = t.Priority,
            IsMandatory = t.IsMandatory,
            Notes = t.Notes,
            CreatedAt = DateTime.SpecifyKind(t.CreatedAt, DateTimeKind.Utc)
        };
    }
}
