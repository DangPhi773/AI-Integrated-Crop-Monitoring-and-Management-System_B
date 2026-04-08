using CMMS.BLL.Helpers;
using CMMS.BLL.Interfaces;
using CMMS.DAL.DBContext;
using CMMS.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMS.BLL.Services
{
    public class NotificationService : INotificationService
    {
        private readonly AppDbContext _db;
        private readonly IEmailService _emailService;
        private readonly IEmailTemplateService _templateService;

        public NotificationService(
            AppDbContext db,
            IEmailService emailService,
            IEmailTemplateService templateService)
        {
            _db = db;
            _emailService = emailService;
            _templateService = templateService;
        }

        public async System.Threading.Tasks.Task NotifyNewWorkerAsync(Guid workerId)
        {
            var worker = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == workerId);
            if (worker == null || string.IsNullOrWhiteSpace(worker.Email)) return;

            var (subject, htmlBody) = _templateService.BuildWelcomeWorkerEmail(worker.Fullname ?? "Worker", worker.Email);
            await _emailService.SendEmailAsync(worker.Email, subject, htmlBody);

            _db.Notifications.Add(new Notification
            {
                NoteId = Guid.NewGuid(),
                UserId = workerId,
                NoteType = "email_welcome",
                NoteTitle = subject,
                NoteMessage = $"Welcome email sent to {worker.Email}",
                NoteStatus = "sent",
                NoteCreatedAt = DateTimeHelper.VnNow()
            });
            await _db.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task NotifyNewReportAsync(Guid reportId)
        {
            var report = await _db.Reports.AsNoTracking().FirstOrDefaultAsync(r => r.ReportId == reportId);
            if (report == null) return;

            string workerName = "Worker";
            if (report.WorkerId.HasValue)
            {
                var worker = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == report.WorkerId);
                if (worker != null) workerName = worker.Fullname ?? worker.Email ?? "Worker";
            }

            var recipients = await _db.Users
                .AsNoTracking()
                .Include(u => u.Role)
                .Where(u => u.Role != null
                            && (u.Role.RoleName == "Owner" || u.Role.RoleName == "Specialist")
                            && u.Email != null)
                .Select(u => new { u.UserId, u.Email })
                .ToListAsync();

            if (recipients.Count == 0) return;

            var emails = recipients.Select(r => r.Email!).Distinct().ToList();
            var submitDateStr = (report.SubmitDate ?? report.CreatedAt ?? DateTimeHelper.VnNow()).ToString("dd/MM/yyyy HH:mm");
            var (subject, htmlBody) = _templateService.BuildNewReportEmail(report.Title ?? "(Không tiêu đề)", workerName, submitDateStr);

            await _emailService.SendEmailAsync(emails, subject, htmlBody);

            var now = DateTimeHelper.VnNow();
            foreach (var r in recipients)
            {
                _db.Notifications.Add(new Notification
                {
                    NoteId = Guid.NewGuid(),
                    UserId = r.UserId,
                    NoteType = "email_new_report",
                    NoteTitle = subject,
                    NoteMessage = $"New report notification sent to {r.Email}",
                    NoteStatus = "sent",
                    NoteCreatedAt = now
                });
            }
            await _db.SaveChangesAsync();
        }
    }
}
