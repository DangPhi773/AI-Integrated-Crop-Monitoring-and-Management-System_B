namespace CMMS.BLL.Configuration
{
    public class WeatherSettings
    {
        public string ApiKey { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = "https://api.openweathermap.org/data/2.5";
        public string Language { get; set; } = "vi";
        public int DefaultForecastDays { get; set; } = 5;
        public string Units { get; set; } = "metric";
    }
}
