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
            WorkerId = r.WorkerId,
            WorkerName = r.Worker?.Fullname,
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
            UpdatedAt = r.UpdatedAt,
            EnvironmentSnapshots = r.EnvironmentSnapshots?.Select(s => new EnvironmentSnapshotDto
            {
                Temperature = s.Temperature,
                Humidity = s.Humidity,
                SoilMoisture = s.SoilMoisture,
                Rainfall = s.Rainfall,
                LightIntensity = s.LightIntensity,
                RecordedAt = s.RecordedAt,
                SourceDeviceId = s.SourceDeviceId
            }).ToList() ?? new()
        };

        public static DiagnosisResponse ToDiagnosisResponse(DiagnosisResult d, string? diagnoserName = null) => new()
        {
            Id = d.Id,
            ReportId = d.ReportId,
            ReportNo = d.Report?.ReportNo,
            ReportTitle = d.Report?.Title,
            DiagnosedBy = d.DiagnosedBy,
            DiagnoserName = diagnoserName ?? d.Diagnoser?.Fullname,
            DiseaseName = d.DiseaseName,
            Conclusion = d.Conclusion,
            RecommendedAction = d.RecommendedAction,
            SeverityLevel = d.SeverityLevel,
            Status = d.Status,
            CreatedAt = d.CreatedAt
        };
    }
}
