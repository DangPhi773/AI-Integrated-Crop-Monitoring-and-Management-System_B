namespace CMMS.DAL.DTOs
{
    public class PlantAnalysisResultDto
    {
        public string PossibleDisease { get; set; } = string.Empty;
        public double Confidence { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<string> SymptomsDetected { get; set; } = new();
        public List<string> CareSuggestions { get; set; } = new();
        public string Severity { get; set; } = string.Empty;
    }
}