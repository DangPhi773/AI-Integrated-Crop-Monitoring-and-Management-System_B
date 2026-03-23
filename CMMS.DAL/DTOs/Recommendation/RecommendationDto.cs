using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.DTOs.Recommendation
{
    public class RecommendationRequest
    {
        public Guid? SeasonId { get; set; }
        public Guid? PestDetectionId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
    }

    public class RecommendationResponse
    {
        public Guid RecommendationId { get; set; }
        public Guid? SeasonId { get; set; }
        public Guid? PestDetectionId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedAt { get; set; }


        public string? PestLabel { get; set; }      
        public string? PestSeverity { get; set; }   
    }
}
