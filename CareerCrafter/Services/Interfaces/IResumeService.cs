using DAL.Models;
using Microsoft.AspNetCore.Http;

namespace Services.Interfaces
{
    public interface IResumeService
    {
        Task<IEnumerable<Resume>> GetAllAsync();
        Task<Resume?> GetByIdAsync(int id);
        Task<Resume> CreateAsync(Resume resume);
        Task<bool> UpdateAsync(int id, Resume resume);
        Task<bool> DeleteAsync(int id);
        Task<string?> UploadResumeAsync(IFormFile file, string userEmail);
    }
}
