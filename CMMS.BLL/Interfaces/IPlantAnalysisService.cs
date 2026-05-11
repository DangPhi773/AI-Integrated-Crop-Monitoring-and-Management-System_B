using CMMS.DAL.DTOs;
using Microsoft.AspNetCore.Http;

namespace CMMS.BLL.Interfaces
{
    public interface IPlantAnalysisService
    {
        Task<PlantAnalysisResultDto> AnalyzePlantImageAsync(
            IFormFile image,
            PlantAnalysisContextDto context);
    }
}