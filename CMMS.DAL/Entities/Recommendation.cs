using System;
using System.Collections.Generic;

namespace CMMS.DAL.Entities;

public partial class Recommendation
{
    public Guid RecommendationId { get; set; }

    public Guid? SeasonId { get; set; }

    public Guid? PestDetectionId { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual PestDetection? PestDetection { get; set; }

    public virtual Season? Season { get; set; }
}
