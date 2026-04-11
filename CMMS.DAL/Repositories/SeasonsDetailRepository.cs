using CMMS.DAL.DBContext;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.Repositories
{
    public class SeasonsDetailRepository : ISeasonsDetailRepository
    {
        private readonly AppDbContext _context;
        public SeasonsDetailRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<SeasonsDetail>> GetAllAsync()
            => await _context.SeasonsDetails
                .Include(sd => sd.Season)
                .Include(sd => sd.Bed)
                .Include(sd => sd.Crop)
                .AsNoTracking()
                .ToListAsync();

        public async Task<SeasonsDetail?> GetByIdAsync(Guid id)
            => await _context.SeasonsDetails
                .Include(sd => sd.Season)
                .Include(sd => sd.Bed)
                .Include(sd => sd.Crop)
                .FirstOrDefaultAsync(sd => sd.SeasonDetailId == id);

        public async System.Threading.Tasks.Task AddAsync(SeasonsDetail seasonsDetail) => await _context.SeasonsDetails.AddAsync(seasonsDetail);

        public void Update(SeasonsDetail seasonsDetail) => _context.SeasonsDetails.Update(seasonsDetail);

        public void Delete(SeasonsDetail seasonsDetail) => _context.SeasonsDetails.Remove(seasonsDetail);

        public async Task<bool> SaveChangesAsync()
            => await _context.SaveChangesAsync() > 0;
    }
}
