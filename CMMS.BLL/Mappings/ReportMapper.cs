using CMMS.DAL.DTOs.Reports.Responses;
using CMMS.DAL.Entities;

namespace CMMS.BLL.Mappings
{
    public static class ReportMapper
    {
        public static ReportResponse ToResponse(Report r) => new()
        {
            ReportId = r.ReportId,
            ReportNo = r.ReportNo,
            CreatedBy = r.CreatedBy,
            CreatorName = r.Creator?.Fullname,
            OwnerId = r.OwnerId,
            OwnerName = r.Owner?.Fullname,
            Title = r.Title,
            Description = r.Description,
            ReportType = r.ReportType,
            PlotId = r.PlotId,
            BedId = r.BedId,
            SeasonId = r.SeasonId,
            AiResultsJson = r.AiResultsJson,
            Status = r.Status,
            CreatedAt = r.CreatedAt,
            SubmitDate = r.SubmitDate,
            UpdatedAt = r.UpdatedAt
        };

        public static DiagnosisResponse ToResponse(DiagnosisResult d, string? diagnoserName = null) => new()
        {
            Id = d.Id,
            ReportId = d.ReportId,
            DiagnosedBy = d.DiagnosedBy,
            DiagnoserName = diagnoserName,
            DiseaseName = d.DiseaseName,
            Conclusion = d.Conclusion,
            RecommendedAction = d.RecommendedAction,
            SeverityLevel = d.SeverityLevel,
            Status = d.Status,
            CreatedAt = d.CreatedAt
        };
    }
}
