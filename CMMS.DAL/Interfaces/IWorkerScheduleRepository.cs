using CMMS.DAL.Entities;

namespace CMMS.DAL.Interfaces
{
    public interface IWorkerScheduleRepository
    {
        Task<IEnumerable<WorkerSchedule>> GetByWorkerIdAsync(Guid workerId);
        Task<IEnumerable<WorkerSchedule>> GetByTaskDetailIdAsync(Guid taskDetailId);
        Task<List<WorkerSchedule>> GetByTaskDetailIdTrackingAsync(Guid taskDetailId);
        Task<WorkerSchedule?> GetByIdAsync(Guid id);
        Task<WorkerSchedule?> GetByTaskDetailAndWorkerAsync(Guid taskDetailId, Guid workerId);
        System.Threading.Tasks.Task AddAsync(WorkerSchedule entity);
        System.Threading.Tasks.Task AddRangeAsync(IEnumerable<WorkerSchedule> entities);
        void Delete(WorkerSchedule entity);
        void DeleteRange(IEnumerable<WorkerSchedule> entities);
        Task<bool> SaveChangesAsync();
    }
}
