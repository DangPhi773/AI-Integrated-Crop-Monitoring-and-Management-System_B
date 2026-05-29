using CMMS.BLL.Helpers;
using CMMS.BLL.Interfaces;
using CMMS.BLL.Realtime;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Notifications;
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
        private readonly INotificationRealtime _realtime;

        public NotificationService(
            IUserRepository userRepo,
            IReportRepository reportRepo,
            INotificationRepository notificationRepo,
            IEmailService emailService,
            IEmailTemplateService templateService,
            INotificationRealtime realtime)
        {
            _userRepo = userRepo;
            _reportRepo = reportRepo;
            _notificationRepo = notificationRepo;
            _emailService = emailService;
            _templateService = templateService;
            _realtime = realtime;
        }

        public async System.Threading.Tasks.Task NotifyNewWorkerAsync(Guid workerId)
        {
            var worker = await _userRepo.GetByIdAsync(workerId);
            if (worker == null || string.IsNullOrWhiteSpace(worker.Email)) return;

            var (subject, htmlBody) = _templateService.BuildWelcomeWorkerEmail(worker.Fullname ?? "Worker", worker.Email);
            await _emailService.SendEmailAsync(worker.Email, subject, htmlBody);

            var notification = new Notification
            {
                NoteId = Guid.NewGuid(),
                UserId = workerId,
                NoteType = "email_welcome",
                NoteTitle = subject,
                NoteMessage = $"Welcome email sent to {worker.Email}",
                NoteStatus = "unread",
                NoteCreatedAt = DateTimeHelper.VnNow()
            };
            await _notificationRepo.AddAsync(notification);
            await _notificationRepo.SaveChangesAsync();

            await _realtime.PushToUserAsync(workerId, new
            {
                noteId = notification.NoteId,
                noteType = notification.NoteType,
                noteTitle = notification.NoteTitle,
                noteMessage = notification.NoteMessage,
                createdAt = notification.NoteCreatedAt
            });
        }

        public async System.Threading.Tasks.Task NotifyAccountApprovedAsync(Guid userId, string roleName)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null || string.IsNullOrWhiteSpace(user.Email)) return;

            var (subject, htmlBody) = _templateService.BuildAccountApprovedEmail(user.Fullname ?? user.Email, roleName, user.Email);
            await _emailService.SendEmailAsync(user.Email, subject, htmlBody);

            var notification = new Notification
            {
                NoteId = Guid.NewGuid(),
                UserId = userId,
                NoteType = "account_approved",
                NoteTitle = subject,
                NoteMessage = $"Tài khoản của bạn đã được phê duyệt với vai trò {roleName}.",
                NoteStatus = "unread",
                NoteCreatedAt = DateTimeHelper.VnNow()
            };
            await _notificationRepo.AddAsync(notification);
            await _notificationRepo.SaveChangesAsync();

            await _realtime.PushToUserAsync(userId, new
            {
                noteId = notification.NoteId,
                noteType = notification.NoteType,
                noteTitle = notification.NoteTitle,
                noteMessage = notification.NoteMessage,
                createdAt = notification.NoteCreatedAt
            });
        }

        public async System.Threading.Tasks.Task NotifyNewReportAsync(Guid reportId)
        {
            var report = await _reportRepo.GetByIdAsync(reportId);
            if (report == null) return;

            string workerName = "Worker";
            if (report.WorkerId.HasValue)
            {
                var worker = await _userRepo.GetByIdAsync(report.WorkerId.Value);
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
                NoteStatus = "unread",
                NoteCreatedAt = now
            }).ToList();

            await _notificationRepo.AddRangeAsync(notifications);
            await _notificationRepo.SaveChangesAsync();

            foreach (var n in notifications)
            {
                await _realtime.PushToUserAsync(n.UserId!.Value, new
                {
                    noteId = n.NoteId,
                    noteType = n.NoteType,
                    noteTitle = n.NoteTitle,
                    noteMessage = n.NoteMessage,
                    reportId = reportId,
                    createdAt = n.NoteCreatedAt
                });
            }
        }

        public async Task<ApiResponse<List<NotificationResponse>>> GetMyNotificationsAsync(Guid userId, bool unreadOnly, int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 20;

            var items = await _notificationRepo.GetByUserIdAsync(userId, unreadOnly, (page - 1) * pageSize, pageSize);
            var data = items.Select(n => new NotificationResponse
            {
                NoteId = n.NoteId,
                ReportId = n.ReportId,
                DiagnosisId = n.DiagnosisId,
                NoteType = n.NoteType,
                NoteTitle = n.NoteTitle,
                NoteMessage = n.NoteMessage,
                NoteStatus = n.NoteStatus,
                NoteCreatedAt = n.NoteCreatedAt
            }).ToList();

            return new ApiResponse<List<NotificationResponse>> { Success = true, Data = data };
        }

        public async Task<ApiResponse<int>> GetUnreadCountAsync(Guid userId)
            => new() { Success = true, Data = await _notificationRepo.CountUnreadAsync(userId) };

        public async Task<ApiResponse<string>> MarkAsReadAsync(Guid noteId, Guid userId)
        {
            var affected = await _notificationRepo.MarkAsReadAsync(noteId, userId);
            return affected > 0
                ? new ApiResponse<string> { Success = true, Message = "Đã đánh dấu đã đọc" }
                : new ApiResponse<string> { Success = false, Message = "Không tìm thấy thông báo" };
        }

        public async Task<ApiResponse<string>> MarkAllAsReadAsync(Guid userId)
        {
            var affected = await _notificationRepo.MarkAllAsReadAsync(userId);
            return new ApiResponse<string> { Success = true, Message = $"Đã đánh dấu {affected} thông báo" };
        }
    }
}
