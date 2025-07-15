using CareerCrafter.DTOs.JobListingDTOs;

namespace Services.Interfaces
{
    public interface IJobListingService
    {
        Task<IEnumerable<JobListingReadDto>> GetAllAsync();
        Task<IEnumerable<JobListingReadDto>> GetAllJobListingsAsync();
        Task<JobListingReadDto?> GetByIdAsync(int id);
        Task<JobListingReadDto> CreateAsync(JobListingCreateDto dto, string username);
        Task<bool> UpdateAsync(int id, JobListingUpdateDto dto,int userId);
        Task<bool> DeleteAsync(int id, string username);
        Task<IEnumerable<JobListingReadDto>> GetMyJobsAsync(int userId);


    }
}
