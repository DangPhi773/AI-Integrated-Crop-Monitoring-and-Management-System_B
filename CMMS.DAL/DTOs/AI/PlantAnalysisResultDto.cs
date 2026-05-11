namespace CMMS.DAL.DTOs
{
    public class PlantAnalysisResultDto
    {
        public string PossibleDisease { get; set; } = string.Empty;
        public double Confidence { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<string> SymptomsDetected { get; set; } = new();
        public List<string> CareSuggestions { get; set; } = new();
        public List<string> TreatmentSteps { get; set; } = new();
        public string Severity { get; set; } = string.Empty;
    }

    public class PlantAnalysisContextDto
    {
        public Guid? FarmId { get; set; }
        public Guid? PlotId { get; set; }
        public Guid? BedId { get; set; }

        public string PlantName { get; set; } = string.Empty;
        public string GrowthStage { get; set; } = string.Empty;

        public double? Temperature { get; set; }
        public double? AirHumidity { get; set; }
        public double? SoilMoisture { get; set; }
        public double? LightIntensity { get; set; }

        public string WeatherCondition { get; set; } = string.Empty;
    }
}