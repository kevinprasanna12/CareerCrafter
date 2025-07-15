using CareerCrafter.DTOs.ResumeDTOs;
using Microsoft.AspNetCore.Http;

namespace Services.Interfaces
{
    public interface IResumeService
    {
        Task<IEnumerable<ResumeReadDto>> GetAllAsync();
        Task<ResumeReadDto?> GetByIdAsync(int id);
        Task<ResumeReadDto> CreateAsync(ResumeCreateDto dto);
        Task<bool> UpdateAsync(int id, ResumeUpdateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<string?> UploadResumeAsync(IFormFile file, int userId);
        Task<(byte[] FileBytes, string ContentType, string FileName)?> DownloadResumeAsync(int resumeId);

    }
}
