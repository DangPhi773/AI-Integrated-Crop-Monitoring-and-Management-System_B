namespace CMMS.DAL.DTOs.Reports.Requests
{
    public class AssignReportRequest
    {
        public Guid AssignedTo { get; set; }
        public string? Note { get; set; }
    }
}
