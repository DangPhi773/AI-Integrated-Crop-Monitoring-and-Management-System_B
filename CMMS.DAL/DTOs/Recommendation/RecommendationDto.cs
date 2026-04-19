using System;

namespace CMMS.DAL.DTOs.Recommendation
{
    public class RecommendationRequest
    {
        public Guid? SeasonId { get; set; }
        public Guid? DiagnosisId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
    }

    public class RecommendationResponse
    {
        public Guid RecommendationId { get; set; }
        public Guid? SeasonId { get; set; }
        public Guid? DiagnosisId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? DiagnosisDiseaseName { get; set; }
        public string? DiagnosisSeverity { get; set; }
    }
}
