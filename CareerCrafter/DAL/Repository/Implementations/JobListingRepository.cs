using DAL.Models;
using DAL.Repository;
using DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class JobListingRepository : Repository<JobListing>, IJobListingRepository
    {
        private readonly CareerCrafterDbContext _context;

        public JobListingRepository(CareerCrafterDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobListing>> GetByEmployerIdAsync(int employerId)
        {
            return await _context.JobListings
                .Where(j => j.EmployerId == employerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<JobListing>> SearchAsync(string keyword, string location)
        {
            return await _context.JobListings
                .Where(j => j.Title.Contains(keyword) || j.Location.Contains(location))
                .ToListAsync();
        }

        public async Task<JobListing> GetWithApplicationsAsync(int jobListingId)
        {
            return await _context.JobListings
                .Include(j => j.Applications)
                .FirstOrDefaultAsync(j => j.JobListingId == jobListingId);
        }
    }
}
