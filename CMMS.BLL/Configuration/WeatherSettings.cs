namespace CMMS.BLL.Configuration
{
    public class WeatherSettings
    {
        public string ApiKey { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = "https://api.weatherapi.com/v1";
        public string Language { get; set; } = "vi";
        public int DefaultForecastDays { get; set; } = 3;
    }
}
