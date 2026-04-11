using CMMS.DAL.DBContext;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;

namespace CMMS.DAL.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context) => _context = context;

        public async System.Threading.Tasks.Task AddAsync(Notification entity) => await _context.Notifications.AddAsync(entity);

        public async System.Threading.Tasks.Task AddRangeAsync(IEnumerable<Notification> entities) => await _context.Notifications.AddRangeAsync(entities);

        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}
