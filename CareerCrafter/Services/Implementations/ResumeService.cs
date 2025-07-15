using CareerCrafter.DTOs.ResumeDTOs;
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

        public async Task<IEnumerable<ResumeReadDto>> GetAllAsync()
        {
            var resumes = await _repository.GetAllAsync();
            return resumes.Select(r => new ResumeReadDto
            {
                ResumeId = r.ResumeId,
                FilePath = Path.GetFileName(r.FilePath),
                JobSeekerId = r.JobSeekerId
            });
        }

        public async Task<ResumeReadDto?> GetByIdAsync(int id)
        {
            var r = await _repository.GetByIdAsync(id);
            return r == null ? null : new ResumeReadDto
            {
                ResumeId = r.ResumeId,
                FilePath = r.FilePath,
                JobSeekerId = r.JobSeekerId
            };
        }

        public async Task<ResumeReadDto> CreateAsync(ResumeCreateDto dto)
        {
            var resume = new Resume
            {
                FilePath = dto.FilePath,
                JobSeekerId = dto.JobSeekerId
            };

            var created = await _repository.AddAsync(resume);

            return new ResumeReadDto
            {
                ResumeId = created.ResumeId,
                FilePath = created.FilePath,
                JobSeekerId = created.JobSeekerId
            };
        }

        public async Task<bool> UpdateAsync(int id, ResumeUpdateDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.FilePath = dto.FilePath;
            existing.JobSeekerId = dto.JobSeekerId;

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

        public async Task<string?> UploadResumeAsync(IFormFile file, int userId)
        {
            if (file == null || file.Length == 0)
                return null;

            var seeker = await _context.JobSeekers.FirstOrDefaultAsync(js => js.UserId == userId); // gets userid
            if (seeker == null)
                return null;

            var resumesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Resumes");
            Directory.CreateDirectory(resumesDirectory);

            var resumePath = Path.Combine(resumesDirectory, Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));

            using (var stream = new FileStream(resumePath, FileMode.Create)) // Create a new file stream to save the resume
            {
                await file.CopyToAsync(stream);
            }

            var existingResume = await _context.Resumes.FirstOrDefaultAsync(r => r.JobSeekerId == seeker.JobSeekerId);

            if (existingResume != null)
            {
                // Delete old file if it exists
                if (File.Exists(existingResume.FilePath))
                    File.Delete(existingResume.FilePath);

                existingResume.FilePath = resumePath;
                _context.Resumes.Update(existingResume);
            }
            else
            {
                var resume = new Resume
                {
                    JobSeekerId = seeker.JobSeekerId,
                    FilePath = resumePath
                };

                _context.Resumes.Add(resume);
            }

            await _context.SaveChangesAsync();

            return resumePath;
        }

        public async Task<(byte[] FileBytes, string ContentType, string FileName)?> DownloadResumeAsync(int resumeId)
        {
            var resume = await _repository.GetByIdAsync(resumeId);
            if (resume == null || string.IsNullOrEmpty(resume.FilePath) || !File.Exists(resume.FilePath))
                return null;

            var fileBytes = await File.ReadAllBytesAsync(resume.FilePath);
            var contentType = "application/pdf"; // or determine dynamically if needed
            var fileName = Path.GetFileName(resume.FilePath);

            return (fileBytes, contentType, fileName);
        }


    }
}
