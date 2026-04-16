namespace CMMS.DAL.DTOs.AI;

public class DiseaseAnalysisResponse
{
    public List<DiseaseResult> Results { get; set; } = new();
    public string? RawResponse { get; set; }
}

public class DiseaseResult
{
    public string Name { get; set; } = null!;
    public string Label { get; set; } = null!;
    public double Score { get; set; }
    public string Severity { get; set; } = null!;
    public List<string> Categories { get; set; } = new();
    public string? Recommendation { get; set; }
}
