using DAL;
using DAL.Models;
using DAL.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services.Implementations
{
    public class ResumeService : IResumeService
    {
        private readonly IResumeRepository _repository;
        private readonly CareerCrafterDbContext _context;

        public ResumeService(IResumeRepository repository, CareerCrafterDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<IEnumerable<Resume>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Resume?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Resume> CreateAsync(Resume resume)
        {
            return await _repository.AddAsync(resume);
        }

        public async Task<bool> UpdateAsync(int id, Resume resume)
        {
            if (id != resume.ResumeId) return false;
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            await _repository.UpdateAsync(resume);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            await _repository.DeleteAsync(id);
            return true;
        }

        public async Task<string?> UploadResumeAsync(IFormFile file, string userEmail)
        {
            if (file == null || file.Length == 0) return null;

            var seeker = await _context.JobSeekers.FirstOrDefaultAsync(js => js.Email == userEmail);
            if (seeker == null) return null;

            var resumePath = Path.Combine("Resumes", Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
            Directory.CreateDirectory("Resumes");

            using (var stream = new FileStream(resumePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var resume = new Resume
            {
                JobSeekerId = seeker.JobSeekerId,
                FilePath = resumePath
            };

            _context.Resumes.Add(resume);
            await _context.SaveChangesAsync();

            return resumePath;
        }
    }
}
