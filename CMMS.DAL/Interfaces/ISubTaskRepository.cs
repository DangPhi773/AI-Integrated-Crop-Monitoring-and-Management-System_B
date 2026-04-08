using CMMS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.Interfaces
{
    public interface ISubTaskRepository
    {
        Task<IEnumerable<SubTask>> GetAllByTaskDetailIdAsync(Guid taskDetailId);
        Task<SubTask?> GetByIdAsync(Guid id);
        System.Threading.Tasks.Task AddAsync(SubTask subTask);
        void Update(SubTask subTask);
        void Delete(SubTask subTask);
        Task<bool> SaveChangesAsync();
    }
}
