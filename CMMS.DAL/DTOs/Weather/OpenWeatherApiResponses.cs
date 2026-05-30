using System.Text.Json.Serialization;

namespace CMMS.DAL.DTOs.Weather
{
    public class OwCoord
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
    }

    public class OwWeather
    {
        public int Id { get; set; }
        public string? Main { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
    }

    public class OwMain
    {
        public double Temp { get; set; }
        [JsonPropertyName("feels_like")] public double FeelsLike { get; set; }
        [JsonPropertyName("temp_min")] public double TempMin { get; set; }
        [JsonPropertyName("temp_max")] public double TempMax { get; set; }
        public int Humidity { get; set; }
        public double Pressure { get; set; }
    }

    public class OwWind
    {
        public double Speed { get; set; }
        public double Deg { get; set; }
        public double? Gust { get; set; }
    }

    public class OwClouds
    {
        public int All { get; set; }
    }

    public class OwPrecip
    {
        [JsonPropertyName("1h")] public double? OneHour { get; set; }
        [JsonPropertyName("3h")] public double? ThreeHour { get; set; }
    }

    public class OwSys
    {
        public string? Country { get; set; }
        public long? Sunrise { get; set; }
        public long? Sunset { get; set; }
    }

    public class OwCurrentResponse
    {
        public OwCoord? Coord { get; set; }
        public List<OwWeather>? Weather { get; set; }
        public OwMain? Main { get; set; }
        public OwWind? Wind { get; set; }
        public OwClouds? Clouds { get; set; }
        public OwPrecip? Rain { get; set; }
        public OwPrecip? Snow { get; set; }
        public int? Visibility { get; set; }
        public long Dt { get; set; }
        public OwSys? Sys { get; set; }
        public int Timezone { get; set; }
        public string? Name { get; set; }
    }

    public class OwForecastCity
    {
        public string? Name { get; set; }
        public string? Country { get; set; }
        public int Timezone { get; set; }
        public long? Sunrise { get; set; }
        public long? Sunset { get; set; }
        public OwCoord? Coord { get; set; }
    }

    public class OwForecastSlot
    {
        public long Dt { get; set; }
        public OwMain? Main { get; set; }
        public List<OwWeather>? Weather { get; set; }
        public OwWind? Wind { get; set; }
        public OwClouds? Clouds { get; set; }
        public OwPrecip? Rain { get; set; }
        public OwPrecip? Snow { get; set; }
        public double Pop { get; set; }
    }

    public class OwForecastResponse
    {
        public List<OwForecastSlot>? List { get; set; }
        public OwForecastCity? City { get; set; }
    }
}
