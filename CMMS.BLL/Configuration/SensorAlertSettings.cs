namespace CMMS.BLL.Configuration
{
    public class SensorAlertSettings
    {
        public bool Enabled { get; set; } = true;
        public int CooldownMinutes { get; set; } = 30;
    }
}
