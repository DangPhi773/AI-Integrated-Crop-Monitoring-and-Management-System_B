using Microsoft.AspNetCore.Http;

namespace CMMS.WebAPI.Controllers
{
    public class PlantImageUploadRequestDto
    {
        public Guid? FarmId { get; set; }
        public Guid? PlotId { get; set; }
        public Guid? BedId { get; set; }

        public string PlantName { get; set; } = string.Empty;
        public string GrowthStage { get; set; } = string.Empty;

        public double? Temperature { get; set; }
        public double? AirHumidity { get; set; }
        public double? SoilMoisture { get; set; }
        public double? LightIntensity { get; set; }

        public string WeatherCondition { get; set; } = string.Empty;

        public IFormFile Image { get; set; } = default!;
    }
}