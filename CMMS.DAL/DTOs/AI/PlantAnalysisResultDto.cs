using System.Text.Json.Serialization;

namespace CMMS.DAL.DTOs
{
    public class PlantAnalysisResultDto
    {
        [JsonPropertyName("disease")]
        public string PossibleDisease { get; set; } = string.Empty;

        [JsonPropertyName("diseaseCode")]
        public string DiseaseCode { get; set; } = string.Empty;

        [JsonPropertyName("confidence")]
        public double Confidence { get; set; }

        [JsonPropertyName("isConfident")]
        public bool IsConfident { get; set; }

        [JsonPropertyName("severity")]
        public string Severity { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("symptoms")]
        public List<string> SymptomsDetected { get; set; } = new();

        [JsonPropertyName("solutions")]
        public List<string> CareSuggestions { get; set; } = new();

        [JsonPropertyName("treatmentSteps")]
        public List<string> TreatmentSteps { get; set; } = new();

        [JsonPropertyName("topPredictions")]
        public List<PlantPredictionDto> TopPredictions { get; set; } = new();

        [JsonPropertyName("modelVersion")]
        public string ModelVersion { get; set; } = string.Empty;

        [JsonPropertyName("english")]
        public PlantDiagnosisLanguageDto English { get; set; } = new();

        [JsonPropertyName("vietnamese")]
        public PlantDiagnosisLanguageDto Vietnamese { get; set; } = new();

        [JsonPropertyName("contextUsed")]
        public PlantAnalysisContextDto? ContextUsed { get; set; }
    }

    public class PlantPredictionDto
    {
        [JsonPropertyName("label")]
        public string Label { get; set; } = string.Empty;

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("confidence")]
        public double Confidence { get; set; }
    }

    public class PlantDiagnosisLanguageDto
    {
        [JsonPropertyName("disease")]
        public string Disease { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("severity")]
        public string Severity { get; set; } = string.Empty;

        [JsonPropertyName("symptoms")]
        public List<string> Symptoms { get; set; } = new();

        [JsonPropertyName("care_suggestions")]
        public List<string> CareSuggestions { get; set; } = new();

        [JsonPropertyName("treatment_steps")]
        public List<string> TreatmentSteps { get; set; } = new();
    }

}