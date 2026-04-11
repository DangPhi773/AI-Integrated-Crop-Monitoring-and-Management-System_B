using CMMS.DAL.Entities;

namespace CMMS.DAL.Interfaces
{
    public interface INotificationRepository
    {
        System.Threading.Tasks.Task AddAsync(Notification entity);
        System.Threading.Tasks.Task AddRangeAsync(IEnumerable<Notification> entities);
        Task<bool> SaveChangesAsync();
    }
}
