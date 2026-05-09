namespace CMMS.BLL.Configuration
{
    public class GoogleMapsSettings
    {
        public string ApiKey { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = "https://maps.googleapis.com/maps/api";
        public string Language { get; set; } = "vi";
        public string Region { get; set; } = "vn";
    }
}
