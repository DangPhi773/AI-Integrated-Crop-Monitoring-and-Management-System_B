using CMMS.DAL.DTOs.Recommendation;
using CMMS.DAL.Entities;

namespace CMMS.BLL.Mappings
{
    public static class RecommendationMapper
    {
        public static RecommendationResponse ToResponse(Recommendation r) => new()
        {
            RecommendationId = r.RecommendationId,
            SeasonId = r.SeasonId,
            DiagnosisId = r.DiagnosisId,
            Title = r.Title,
            Content = r.Content,
            CreatedAt = r.CreatedAt,
            DiagnosisDiseaseName = r.Diagnosis?.DiseaseName,
            DiagnosisSeverity = r.Diagnosis?.SeverityLevel
        };
    }
}
