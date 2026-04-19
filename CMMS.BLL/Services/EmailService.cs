using CMMS.BLL.Configuration;
using CMMS.BLL.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMS.BLL.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public Task SendEmailAsync(string toEmail, string subject, string htmlBody)
            => SendEmailAsync(new List<string> { toEmail }, subject, htmlBody);

        public async Task SendEmailAsync(List<string> toEmails, string subject, string htmlBody)
        {
            if (toEmails == null || toEmails.Count == 0) return;

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            foreach (var to in toEmails)
            {
                if (!string.IsNullOrWhiteSpace(to))
                    message.To.Add(MailboxAddress.Parse(to));
            }
            if (message.To.Count == 0) return;

            message.Subject = subject;
            message.Body = new BodyBuilder { HtmlBody = htmlBody }.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_settings.SmtpHost, _settings.SmtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_settings.SenderEmail, _settings.SenderPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
