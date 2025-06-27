using DAL.Models;
using DAL.Repository;
using DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ApplicationRepository : Repository<Application>, IApplicationRepository
    {
        private readonly CareerCrafterDbContext _context;

        public ApplicationRepository(CareerCrafterDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Application>> GetByJobSeekerIdAsync(int jobSeekerId)
        {
            return await _context.Applications
                .Where(a => a.JobSeekerId == jobSeekerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Application>> GetByJobListingIdAsync(int jobListingId)
        {
            return await _context.Applications
                .Where(a => a.JobListingId == jobListingId)
                .ToListAsync();
        }
    }
}
