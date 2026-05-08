using CMMS.DAL.DTOs.Weather;

namespace CMMS.BLL.Interfaces
{
    public interface IWeatherService
    {
        Task<WeatherCurrentDto> GetCurrentAsync(decimal latitude, decimal longitude);
        Task<WeatherForecastDto> GetForecastAsync(decimal latitude, decimal longitude, int days);
        Task<WeatherCurrentDto> GetCurrentByFarmAsync(Guid farmId);
        Task<WeatherForecastDto> GetForecastByFarmAsync(Guid farmId, int days);
    }
}
