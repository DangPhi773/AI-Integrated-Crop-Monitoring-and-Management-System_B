using CMMS.DAL.Entities;

namespace CMMS.DAL.Interfaces
{
    public interface IAttachmentRepository
    {
        System.Threading.Tasks.Task AddAsync(Attachment entity);
        Task<Attachment?> GetByIdAsync(Guid id);
        Task<List<Attachment>> GetByObjectAsync(string objectType, Guid objectId);
        Task<bool> SaveChangesAsync();
    }
}
