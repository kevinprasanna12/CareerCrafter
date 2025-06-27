using DAL.Models;

namespace Services.Interfaces
{
    public interface IJobListingService
    {
        Task<IEnumerable<JobListing>> GetAllAsync();
        Task<JobListing?> GetByIdAsync(int id);
        Task<JobListing> CreateAsync(JobListing jobListing);
        Task<bool> UpdateAsync(int id, JobListing jobListing, string username);
        Task<bool> DeleteAsync(int id, string username);
        Task<IEnumerable<JobListing>> GetMyJobsAsync(string username);
    }
}
