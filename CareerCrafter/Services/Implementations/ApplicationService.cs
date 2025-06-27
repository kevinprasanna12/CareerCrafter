using DAL;
using DAL.Models;
using DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services.Implementations
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _repository;
        private readonly CareerCrafterDbContext _context;

        public ApplicationService(IApplicationRepository repository, CareerCrafterDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<IEnumerable<Application>> GetAllApplicationsAsync() => await _repository.GetAllAsync();

        public async Task<Application?> GetApplicationByIdAsync(int id)
            => await _repository.GetByIdAsync(id);

        public async Task<Application> CreateApplicationAsync(Application application)
            => await _repository.AddAsync(application);

        public async Task<bool> UpdateApplicationAsync(int id, Application application)
        {
            if (id != application.ApplicationId) return false;

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            await _repository.UpdateAsync(application);
            return true;
        }

        public async Task<bool> DeleteApplicationAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            await _repository.DeleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<Application>> GetApplicationsByJobSeekerAsync(string email)
        {
            var seeker = await _context.JobSeekers.FirstOrDefaultAsync(js => js.Email == email);
            if (seeker == null) return Enumerable.Empty<Application>();

            return await _context.Applications
                .Include(a => a.JobListing)
                .Where(a => a.JobSeekerId == seeker.JobSeekerId)
                .ToListAsync();
        }

        public async Task<(bool Success, string Message)> ApplyToJobAsync(string email, int jobListingId)
        {
            var seeker = await _context.JobSeekers.FirstOrDefaultAsync(js => js.Email == email);
            if (seeker == null) return (false, "Job Seeker not found");

            var resume = await _context.Resumes.FirstOrDefaultAsync(r => r.JobSeekerId == seeker.JobSeekerId);
            if (resume == null) return (false, "You must upload a resume before applying.");

            var existing = await _context.Applications.FirstOrDefaultAsync(a =>
                a.JobSeekerId == seeker.JobSeekerId && a.JobListingId == jobListingId);
            if (existing != null) return (false, "You already applied to this job.");

            var application = new Application
            {
                JobSeekerId = seeker.JobSeekerId,
                JobListingId = jobListingId,
                AppliedDate = DateTime.UtcNow
            };

            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            return (true, "Application submitted successfully.");
        }
    }
}
