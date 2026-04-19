namespace CMMS.BLL.Interfaces
{
    public interface IEmailTemplateService
    {
        (string subject, string htmlBody) BuildWelcomeWorkerEmail(string workerName, string workerEmail);
        (string subject, string htmlBody) BuildNewReportEmail(string reportTitle, string workerName, string submitDate);
    }
}
