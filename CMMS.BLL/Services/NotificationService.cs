using CMMS.BLL.Helpers;
using CMMS.BLL.Interfaces;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;

namespace CMMS.BLL.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUserRepository _userRepo;
        private readonly IReportRepository _reportRepo;
        private readonly INotificationRepository _notificationRepo;
        private readonly IEmailService _emailService;
        private readonly IEmailTemplateService _templateService;

        public NotificationService(
            IUserRepository userRepo,
            IReportRepository reportRepo,
            INotificationRepository notificationRepo,
            IEmailService emailService,
            IEmailTemplateService templateService)
        {
            _userRepo = userRepo;
            _reportRepo = reportRepo;
            _notificationRepo = notificationRepo;
            _emailService = emailService;
            _templateService = templateService;
        }

        public async System.Threading.Tasks.Task NotifyNewWorkerAsync(Guid workerId)
        {
            var worker = await _userRepo.GetByIdAsync(workerId);
            if (worker == null || string.IsNullOrWhiteSpace(worker.Email)) return;

            var (subject, htmlBody) = _templateService.BuildWelcomeWorkerEmail(worker.Fullname ?? "Worker", worker.Email);
            await _emailService.SendEmailAsync(worker.Email, subject, htmlBody);

            await _notificationRepo.AddAsync(new Notification
            {
                NoteId = Guid.NewGuid(),
                UserId = workerId,
                NoteType = "email_welcome",
                NoteTitle = subject,
                NoteMessage = $"Welcome email sent to {worker.Email}",
                NoteStatus = "sent",
                NoteCreatedAt = DateTimeHelper.VnNow()
            });
            await _notificationRepo.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task NotifyNewReportAsync(Guid reportId)
        {
            var report = await _reportRepo.GetByIdAsync(reportId);
            if (report == null) return;

            string workerName = "Worker";
            if (report.CreatedBy.HasValue)
            {
                var worker = await _userRepo.GetByIdAsync(report.CreatedBy.Value);
                if (worker != null) workerName = worker.Fullname ?? worker.Email ?? "Worker";
            }

            var recipients = await _userRepo.GetByRoleNamesAsync("Owner", "Specialist");
            if (recipients.Count == 0) return;

            var emails = recipients.Select(r => r.Email!).Distinct().ToList();
            var submitDateStr = (report.SubmitDate ?? report.CreatedAt ?? DateTimeHelper.VnNow()).ToString("dd/MM/yyyy HH:mm");
            var (subject, htmlBody) = _templateService.BuildNewReportEmail(report.Title ?? "(Không tiêu đề)", workerName, submitDateStr);

            await _emailService.SendEmailAsync(emails, subject, htmlBody);

            var now = DateTimeHelper.VnNow();
            var notifications = recipients.Select(r => new Notification
            {
                NoteId = Guid.NewGuid(),
                UserId = r.UserId,
                NoteType = "email_new_report",
                NoteTitle = subject,
                NoteMessage = $"New report notification sent to {r.Email}",
                NoteStatus = "sent",
                NoteCreatedAt = now
            });

            await _notificationRepo.AddRangeAsync(notifications);
            await _notificationRepo.SaveChangesAsync();
        }
    }
}
