using CMMS.DAL.DBContext;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMMS.DAL.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context) => _context = context;

        public async System.Threading.Tasks.Task AddAsync(Notification entity) => await _context.Notifications.AddAsync(entity);

        public async System.Threading.Tasks.Task AddRangeAsync(IEnumerable<Notification> entities) => await _context.Notifications.AddRangeAsync(entities);

        public async Task<List<Notification>> GetByUserIdAsync(Guid userId, bool unreadOnly, int skip, int take)
        {
            var query = _context.Notifications.AsNoTracking().Where(n => n.UserId == userId);
            if (unreadOnly) query = query.Where(n => n.NoteStatus == "unread");
            return await query.OrderByDescending(n => n.NoteCreatedAt).Skip(skip).Take(take).ToListAsync();
        }

        public async Task<int> CountUnreadAsync(Guid userId)
            => await _context.Notifications.CountAsync(n => n.UserId == userId && n.NoteStatus == "unread");

        public async Task<int> MarkAsReadAsync(Guid noteId, Guid userId)
            => await _context.Notifications
                .Where(n => n.NoteId == noteId && n.UserId == userId && n.NoteStatus != "read")
                .ExecuteUpdateAsync(s => s.SetProperty(n => n.NoteStatus, "read"));

        public async Task<int> MarkAllAsReadAsync(Guid userId)
            => await _context.Notifications
                .Where(n => n.UserId == userId && n.NoteStatus != "read")
                .ExecuteUpdateAsync(s => s.SetProperty(n => n.NoteStatus, "read"));

        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}
