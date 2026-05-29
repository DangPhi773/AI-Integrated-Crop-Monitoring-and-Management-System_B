using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Notifications;

namespace CMMS.BLL.Interfaces
{
    public interface INotificationService
    {
        Task NotifyNewWorkerAsync(Guid workerId);
        Task NotifyNewReportAsync(Guid reportId);
        Task<ApiResponse<List<NotificationResponse>>> GetMyNotificationsAsync(Guid userId, bool unreadOnly, int page, int pageSize);
        Task<ApiResponse<int>> GetUnreadCountAsync(Guid userId);
        Task<ApiResponse<string>> MarkAsReadAsync(Guid noteId, Guid userId);
        Task<ApiResponse<string>> MarkAllAsReadAsync(Guid userId);
    }
}
