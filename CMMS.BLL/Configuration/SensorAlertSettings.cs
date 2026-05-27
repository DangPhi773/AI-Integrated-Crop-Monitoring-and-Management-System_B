namespace CMMS.BLL.Configuration
{
    public class SensorAlertSettings
    {
        public bool Enabled { get; set; } = true;
        public double LowHumidityPct { get; set; } = 40;
        public double HighHumidityPct { get; set; } = 95;
        public double LowTemperatureC { get; set; } = 10;
        public double HighTemperatureC { get; set; } = 35;
        public double LowSoilMoisturePct { get; set; } = 30;
        public double HighSoilMoisturePct { get; set; } = 90;
        public double HighLightLux { get; set; } = 80000;
        public int CooldownMinutes { get; set; } = 30;
    }
}
