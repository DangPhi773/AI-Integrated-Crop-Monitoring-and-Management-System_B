using CMMS.DAL.Entities;
using System;
using System.Collections.Generic;

namespace CMMS.DAL.Interfaces
{
    public interface ITaskDetailRepository
    {
        System.Threading.Tasks.Task<IEnumerable<TaskDetail>> GetAllAsync();
        System.Threading.Tasks.Task<TaskDetail?> GetByIdAsync(Guid id);
        System.Threading.Tasks.Task<IEnumerable<TaskDetail>> GetBySeasonIdAsync(Guid seasonId);
        System.Threading.Tasks.Task<IEnumerable<TaskDetail>> GetByWorkerIdAsync(Guid workerId);
        System.Threading.Tasks.Task<IEnumerable<TaskDetail>> GetByBedIdAsync(Guid bedId);
        System.Threading.Tasks.Task<IEnumerable<TaskDetail>> GetByTaskIdAsync(Guid taskId);
        System.Threading.Tasks.Task<List<TaskDetail>> GetActiveOverlappingAsync(DateTime start, DateTime end, Guid? excludeTaskDetailId);
        System.Threading.Tasks.Task AddAsync(TaskDetail entity);
        void Update(TaskDetail entity);
        void Delete(TaskDetail entity);
        System.Threading.Tasks.Task<bool> SaveChangesAsync();
    }
}
