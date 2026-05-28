using System;
using System.ComponentModel.DataAnnotations;

namespace CMMS.DAL.DTOs.Recommendation
{
    public class RecommendationRequest
    {
        public Guid? SeasonId { get; set; }
        public Guid? DiagnosisId { get; set; }

        [StringLength(200, MinimumLength = 1, ErrorMessage = "Title phải từ 1 đến 200 ký tự")]
        public string? Title { get; set; }

        [StringLength(5000, ErrorMessage = "Content tối đa 5000 ký tự")]
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
