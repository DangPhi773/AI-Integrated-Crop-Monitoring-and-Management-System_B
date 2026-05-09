namespace CMMS.DAL.DTOs.Weather
{
    public class WeatherForecastDayDto
    {
        public DateTime Date { get; set; }
        public double MaxTempC { get; set; }
        public double MinTempC { get; set; }
        public double AvgTempC { get; set; }
        public double TotalPrecipMm { get; set; }
        public int AvgHumidity { get; set; }
        public double MaxWindKph { get; set; }
        public double Uv { get; set; }
        public int ChanceOfRain { get; set; }
        public WeatherConditionDto Condition { get; set; } = new();
        public string? Sunrise { get; set; }
        public string? Sunset { get; set; }
    }

    public class WeatherForecastDto
    {
        public WeatherLocationDto Location { get; set; } = new();
        public WeatherCurrentDto Current { get; set; } = new();
        public List<WeatherForecastDayDto> Forecast { get; set; } = new();
    }
}
