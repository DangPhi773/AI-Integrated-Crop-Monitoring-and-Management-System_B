namespace CMMS.DAL.DTOs.Weather
{
    public class WeatherLocationDto
    {
        public string? Name { get; set; }
        public string? Region { get; set; }
        public string? Country { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string? LocalTime { get; set; }
    }

    public class WeatherConditionDto
    {
        public string? Text { get; set; }
        public string? Icon { get; set; }
        public int Code { get; set; }
    }

    public class WeatherCurrentDto
    {
        public WeatherLocationDto Location { get; set; } = new();
        public DateTime LastUpdated { get; set; }
        public double TempC { get; set; }
        public double FeelsLikeC { get; set; }
        public int Humidity { get; set; }
        public double WindKph { get; set; }
        public string? WindDir { get; set; }
        public double GustKph { get; set; }
        public double PrecipMm { get; set; }
        public double PressureMb { get; set; }
        public int Cloud { get; set; }
        public double Uv { get; set; }
        public double VisKm { get; set; }
        public bool IsDay { get; set; }
        public WeatherConditionDto Condition { get; set; } = new();
    }
}
