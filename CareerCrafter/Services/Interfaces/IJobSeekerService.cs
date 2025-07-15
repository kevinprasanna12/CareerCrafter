using CareerCrafter.DTOs.JobSeekerDTOs;
using DAL.Models;

namespace Services.Interfaces
{
    public interface IJobSeekerService
    {
        Task<IEnumerable<JobSeekerReadDto>> GetAllAsync();
        Task<JobSeekerReadDto?> GetByIdAsync(int id);
        Task<JobSeekerReadDto> CreateAsync(JobSeekerCreateDto dto,int userId);
        Task<bool> UpdateAsync(int id, JobSeekerUpdateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<JobSeekerReadDto> GetByUserIdAsync(int userId);

            
    }
}
