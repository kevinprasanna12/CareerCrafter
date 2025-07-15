using CareerCrafter.DTOs.ApplicationDTOs;

namespace Services.Interfaces
{
    public interface IApplicationService
    {
        Task<IEnumerable<ApplicationReadDto>> GetAllApplicationsAsync();
        Task<ApplicationReadDto?> GetApplicationByIdAsync(int id);
        Task<ApplicationReadDto> CreateApplicationAsync(ApplicationCreateDto dto);
        Task<bool> UpdateApplicationAsync(int id, ApplicationUpdateDto dto);
        Task<bool> DeleteApplicationAsync(int id);
        Task<IEnumerable<ApplicationReadDto>> GetApplicationsByJobSeekerAsync(int userId);
        Task<(bool Success, string Message)> ApplyToJobAsync(int userId, int jobListingId);
        Task<IEnumerable<EmployerApplicationReadDto>> GetApplicationsForEmployerAsync(int userId);
        Task<JobListingApplicationCountDto> GetJobListingApplicationCountsAsync(int userId);
        Task<int> GetApplicationCountForJobSeekerAsync(int userId);


    }
}
