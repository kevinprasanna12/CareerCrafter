using DAL;
using DAL.Models;
using DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services.Implementations
{
    public class JobListingService : IJobListingService
    {
        private readonly IJobListingRepository _repository;
        private readonly CareerCrafterDbContext _context;

        public JobListingService(IJobListingRepository repository, CareerCrafterDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<IEnumerable<JobListing>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<JobListing?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<JobListing> CreateAsync(JobListing jobListing)
        {
            return await _repository.AddAsync(jobListing);
        }

        public async Task<bool> UpdateAsync(int id, JobListing jobListing, string username)
        {
            if (id != jobListing.JobListingId)
                return false;

            var employer = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.Role == "Employer");

            if (employer == null)
                return false;

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null || existing.EmployerId != employer.UserId)
                return false;

            await _repository.UpdateAsync(jobListing);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, string username)
        {
            var employer = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.Role == "Employer");

            if (employer == null)
                return false;

            var job = await _repository.GetByIdAsync(id);
            if (job == null || job.EmployerId != employer.UserId)
                return false;

            await _repository.DeleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<JobListing>> GetMyJobsAsync(string username)
        {
            var employer = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.Role == "Employer");

            if (employer == null)
                return new List<JobListing>();

            return await _context.JobListings
                .Where(j => j.EmployerId == employer.UserId)
                .ToListAsync();
        }
    }
}
