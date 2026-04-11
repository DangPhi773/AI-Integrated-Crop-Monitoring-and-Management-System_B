using CMMS.DAL.DBContext;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMMS.DAL.Repositories
{
    public class AttachmentRepository : IAttachmentRepository
    {
        private readonly AppDbContext _context;

        public AttachmentRepository(AppDbContext context) => _context = context;

        public async System.Threading.Tasks.Task AddAsync(Attachment entity) => await _context.Attachments.AddAsync(entity);

        public async Task<Attachment?> GetByIdAsync(Guid id) => await _context.Attachments.FindAsync(id);

        public async Task<List<Attachment>> GetByObjectAsync(string objectType, Guid objectId)
        {
            return await _context.Attachments
                .AsNoTracking()
                .Where(a => a.ObjectType == objectType && a.ObjectId == objectId && !a.IsDeleted)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}
