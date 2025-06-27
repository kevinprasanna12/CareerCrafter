using DAL.Models;
using DAL.Repository.Interfaces;
using Services.Interfaces;

namespace Services.Implementations
{
    public class JobSeekerService : IJobSeekerService
    {
        private readonly IJobSeekerRepository _repository;

        public JobSeekerService(IJobSeekerRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<JobSeeker>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<JobSeeker?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<JobSeeker> CreateAsync(JobSeeker jobSeeker)
        {
            return await _repository.AddAsync(jobSeeker);
        }

        public async Task<bool> UpdateAsync(int id, JobSeeker jobSeeker)
        {
            if (id != jobSeeker.JobSeekerId) return false;

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            await _repository.UpdateAsync(jobSeeker);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
