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
    public class ReportRepository : IReportRepository
    {
        private readonly AppDbContext _context;

        public ReportRepository(AppDbContext context)
        {
            _context = context;
        }
        public async System.Threading.Tasks.Task AddAsync(Report report)
        {
            await _context.Reports.AddAsync(report);
        }

        public void Delete(Report report)
        {
            _context.Reports.Remove(report);
        }

        public async Task<IEnumerable<Report>> GetAllAsync()
        {
            return await _context.Reports.ToListAsync();
        }

        public async Task<IEnumerable<Report>> GetAllWithWorkerAsync()
        {
            return await _context.Reports
                .Include(r => r.Worker) 
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<Report?> GetByIdAsync(Guid id)
        {
            return await _context.Reports.FindAsync(id);
        }

        public async Task<Report?> GetByIdWithWorkerAsync(Guid id)
        {
            return await _context.Reports
                .Include(r => r.Worker)
                .FirstOrDefaultAsync(r => r.ReportId == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(Report report)
        {
            _context.Reports.Update(report);
        }
    }
}
