namespace CMMS.BLL.Configuration
{
    public class PlantNetSettings
    {
        public string ApiKey { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = "https://my-api.plantnet.org/v2";
        public string DefaultLang { get; set; } = "vi";
    }
}
