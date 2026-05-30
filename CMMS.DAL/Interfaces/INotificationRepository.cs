using CMMS.DAL.Entities;

namespace CMMS.DAL.Interfaces
{
    public interface INotificationRepository
    {
        System.Threading.Tasks.Task AddAsync(Notification entity);
        System.Threading.Tasks.Task AddRangeAsync(IEnumerable<Notification> entities);
        Task<(List<Notification> Items, int Total)> GetByUserIdAsync(Guid userId, bool unreadOnly, int skip, int take);
        Task<int> CountUnreadAsync(Guid userId);
        Task<int> MarkAsReadAsync(Guid noteId, Guid userId);
        Task<int> MarkAllAsReadAsync(Guid userId);
        Task<bool> SaveChangesAsync();
    }
}
