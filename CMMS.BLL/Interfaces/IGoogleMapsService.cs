using CMMS.DAL.DTOs.Maps;

namespace CMMS.BLL.Interfaces
{
    public interface IGoogleMapsService
    {
        Task<GeocodeResultDto> GeocodeAsync(string address);
        Task<GeocodeResultDto> ReverseGeocodeAsync(decimal latitude, decimal longitude);
        Task<GeocodeResultDto> UpdateFarmCoordinatesAsync(Guid farmId, UpdateFarmCoordinatesRequestDto request);
    }
}
