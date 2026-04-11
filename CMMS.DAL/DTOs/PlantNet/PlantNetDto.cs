namespace CMMS.DAL.DTOs.PlantNet
{
    public class PlantNetDiseaseResponse
    {
        public List<PlantNetDiseaseResult> Results { get; set; } = new();
        public int RemainingRequests { get; set; }
        public string? Version { get; set; }
    }

    public class PlantNetDiseaseResult
    {
        public string Name { get; set; } = null!;
        public string Label { get; set; } = null!;
        public double Score { get; set; }
        public string Severity { get; set; } = null!;
        public List<string> Categories { get; set; } = new();
    }
}
