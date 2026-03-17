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
    public class BedRepository : IBedRepository
    {
        private readonly AppDbContext _context;
        public BedRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Bed>> GetAllAsync()
            => await _context.Beds
                .Include(b => b.Plot)
                .Include(b => b.SeasonsDetails)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Bed?> GetByIdAsync(Guid id)
            => await _context.Beds
                .Include(b => b.Plot)
                .Include(b => b.SeasonsDetails)
                .FirstOrDefaultAsync(b => b.BedId == id);

        public async System.Threading.Tasks.Task AddAsync(Bed bed) => await _context.Beds.AddAsync(bed);

        public void Update(Bed bed) => _context.Beds.Update(bed);

        public void Delete(Bed bed) => _context.Beds.Remove(bed);

        public async Task<bool> SaveChangesAsync()
            => await _context.SaveChangesAsync() > 0;
    }
}
