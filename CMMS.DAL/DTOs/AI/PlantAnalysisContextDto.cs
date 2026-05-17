using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace CMMS.DAL.DTOs
{
    public class PlantAnalysisContextDto
    {
        [JsonPropertyName("farmId")]
        public Guid? FarmId { get; set; }

        [JsonPropertyName("plotId")]
        public Guid? PlotId { get; set; }

        [JsonPropertyName("bedId")]
        public Guid? BedId { get; set; }

        [JsonPropertyName("plantName")]
        public string? PlantName { get; set; }

        [JsonPropertyName("growthStage")]
        public string? GrowthStage { get; set; }

        [JsonPropertyName("temperature")]
        public double? Temperature { get; set; }

        [JsonPropertyName("airHumidity")]
        public double? AirHumidity { get; set; }

        [JsonPropertyName("soilMoisture")]
        public double? SoilMoisture { get; set; }

        [JsonPropertyName("lightIntensity")]
        public double? LightIntensity { get; set; }

        [JsonPropertyName("weatherCondition")]
        public string? WeatherCondition { get; set; }
    }

    public class PlantAnalysisRequestDto
    {
        public Guid? FarmId { get; set; }
        public Guid? PlotId { get; set; }
        public Guid? BedId { get; set; }
        public string? PlantName { get; set; }
        public string? GrowthStage { get; set; }
        public double? Temperature { get; set; }
        public double? AirHumidity { get; set; }
        public double? SoilMoisture { get; set; }
        public double? LightIntensity { get; set; }
        public string? WeatherCondition { get; set; }
        public IFormFile? Image { get; set; }
    }
}