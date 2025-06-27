using DAL.Models;

namespace Services.Interfaces
{
    public interface IJobSeekerService
    {
        Task<IEnumerable<JobSeeker>> GetAllAsync();
        Task<JobSeeker?> GetByIdAsync(int id);
        Task<JobSeeker> CreateAsync(JobSeeker jobSeeker);
        Task<bool> UpdateAsync(int id, JobSeeker jobSeeker);
        Task<bool> DeleteAsync(int id);
    }
}
