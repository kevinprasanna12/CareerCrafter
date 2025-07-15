using CareerCrafter.DTOs.JobSeekerDTOs;
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

        public async Task<IEnumerable<JobSeekerReadDto>> GetAllAsync()
        {
            var seekers = await _repository.GetAllAsync();
            return seekers.Select(s => new JobSeekerReadDto
            {
                JobSeekerId = s.JobSeekerId,
                FullName = s.FullName,
                Email = s.Email
            });
        }

        public async Task<JobSeekerReadDto?> GetByIdAsync(int id)
        {
            var s = await _repository.GetByIdAsync(id);
            if (s == null) return null;

            return new JobSeekerReadDto
            {
                JobSeekerId = s.JobSeekerId,
                FullName = s.FullName,
                Email = s.Email
            };
        }

        public async Task<JobSeekerReadDto> CreateAsync(JobSeekerCreateDto dto, int userId)
        {
            var seeker = new JobSeeker
            {
                FullName = dto.FullName,
                Email = dto.Email,
                UserId = userId
            };

            var created = await _repository.AddAsync(seeker);

            return new JobSeekerReadDto
            {
                JobSeekerId = created.JobSeekerId,
                FullName = created.FullName,
                Email = created.Email
            };
        }

        public async Task<bool> UpdateAsync(int id, JobSeekerUpdateDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.FullName = dto.FullName;
            existing.Email = dto.Email;

            await _repository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            await _repository.DeleteAsync(id);
            return true;
        }

        public async Task<JobSeekerReadDto> GetByUserIdAsync(int userId)
        {
            var jobSeeker = await _repository.GetByUserIdAsync(userId);
            if (jobSeeker == null)
                return null;

            return new JobSeekerReadDto
            {
                JobSeekerId = jobSeeker.JobSeekerId,
                FullName = jobSeeker.FullName,
                Email = jobSeeker.Email,
                
                
            };
        }

    }
}
