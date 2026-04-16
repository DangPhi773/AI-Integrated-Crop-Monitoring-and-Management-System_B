using CMMS.DAL.DTOs.AI;
using Microsoft.AspNetCore.Http;

namespace CMMS.BLL.Interfaces;

public interface IGeminiDiseaseService
{
    Task<DiseaseAnalysisResponse> AnalyzeImageAsync(IFormFile image, string? organ = null);
}
