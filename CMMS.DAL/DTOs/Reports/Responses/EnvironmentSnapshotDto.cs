namespace CMMS.DAL.DTOs.Reports.Responses
{
    public class EnvironmentSnapshotDto
    {
        public double? Temperature { get; set; }
        public double? Humidity { get; set; }
        public double? SoilMoisture { get; set; }
        public double? Rainfall { get; set; }
        public double? LightIntensity { get; set; }
        public DateTime RecordedAt { get; set; }
        public Guid? SourceDeviceId { get; set; }
    }
}
