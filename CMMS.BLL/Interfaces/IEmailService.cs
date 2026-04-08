using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMS.BLL.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string htmlBody);
        Task SendEmailAsync(List<string> toEmails, string subject, string htmlBody);
    }
}
