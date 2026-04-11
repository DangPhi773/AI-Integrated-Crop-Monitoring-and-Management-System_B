using CMMS.DAL.DTOs.PlantNet;
using Microsoft.AspNetCore.Http;

namespace CMMS.BLL.Interfaces
{
    public interface IPlantNetService
    {
        Task<PlantNetDiseaseResponse> IdentifyDiseaseAsync(IFormFile image, string organ = "auto");
    }
}
