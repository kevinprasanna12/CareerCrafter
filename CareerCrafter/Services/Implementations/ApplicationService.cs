using CareerCrafter.DTOs.ApplicationDTOs;
using CareerCrafter.DTOs.ResumeDTOs;
using DAL;
using DAL.Models;
using DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services.Implementations
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _repository;
        private readonly CareerCrafterDbContext _context;

        public ApplicationService(IApplicationRepository repository, CareerCrafterDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<IEnumerable<ApplicationReadDto>> GetAllApplicationsAsync()
        {
            var apps = await _repository.GetAllAsync();
            return apps.Select(a => new ApplicationReadDto
            {
                ApplicationId = a.ApplicationId,
                JobSeekerId = a.JobSeekerId,
                JobListingId = a.JobListingId,
                AppliedDate = a.AppliedDate,
                Status = a.Status,
                companyName = a.JobListing.Employer.CompanyName
            });
        }

        public async Task<ApplicationReadDto?> GetApplicationByIdAsync(int id)
        {
            var a = await _repository.GetByIdAsync(id);
            return a == null ? null : new ApplicationReadDto
            {
                ApplicationId = a.ApplicationId,
                JobSeekerId = a.JobSeekerId,
                JobListingId = a.JobListingId,
                AppliedDate = a.AppliedDate,
                Status = a.Status
            };
        }

        public async Task<ApplicationReadDto> CreateApplicationAsync(ApplicationCreateDto dto)
        {
            var app = new Application
            {
                JobSeekerId = dto.JobSeekerId,
                JobListingId = dto.JobListingId,
                AppliedDate = dto.AppliedDate,
                Status = dto.Status
            };

            var created = await _repository.AddAsync(app);

            return new ApplicationReadDto
            {
                ApplicationId = created.ApplicationId,
                JobSeekerId = created.JobSeekerId,
                JobListingId = created.JobListingId,
                AppliedDate = created.AppliedDate,
                Status = created.Status
            };
        }

        public async Task<bool> UpdateApplicationAsync(int id, ApplicationUpdateDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.Status = dto.Status;

            await _repository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteApplicationAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            await _repository.DeleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<ApplicationReadDto>> GetApplicationsByJobSeekerAsync(int userId)
        {
            var seeker = await _context.JobSeekers.FirstOrDefaultAsync(js => js.UserId == userId);
            if (seeker == null) return Enumerable.Empty<ApplicationReadDto>();

            var apps = await _context.Applications
                .Where(a => a.JobSeekerId == seeker.JobSeekerId)
                .Include(a => a.JobListing)
                    .ThenInclude(jl => jl.Employer)
                .Include(a => a.JobSeeker)
                    .ThenInclude(js => js.Resume)
                .ToListAsync();

            return apps.Select(a => new ApplicationReadDto
            {
                ApplicationId = a.ApplicationId,
                JobSeekerId = a.JobSeekerId,
                JobListingId = a.JobListingId,
                AppliedDate = a.AppliedDate,
                Status = a.Status,
                companyName = a.JobListing?.Employer?.CompanyName ?? "Unknown",
                JobTitle = a.JobListing?.Title ?? "Unknown",
                Resume = a.JobSeeker.Resume == null ? null : new ResumeReadDto
                {
                    ResumeId = a.JobSeeker.Resume.ResumeId,
                    FilePath = a.JobSeeker.Resume.FilePath,
                    JobSeekerId = a.JobSeeker.Resume.JobSeekerId
                }
            });
        }


        // Apply to a job listing(job seeker must have a resume)
        public async Task<(bool Success, string Message)> ApplyToJobAsync(int userId, int jobListingId)
        {
            var seeker = await _context.JobSeekers.FirstOrDefaultAsync(js => js.UserId == userId);

            var resume = await _context.Resumes.FirstOrDefaultAsync(r => r.JobSeekerId == seeker.JobSeekerId);
            if (resume == null) return (false, "You must upload a resume before applying.");

            var existing = await _context.Applications.FirstOrDefaultAsync(a =>
                a.JobSeekerId == seeker.JobSeekerId && a.JobListingId == jobListingId);
            if (existing != null) return (false, "You already applied to this job.");

            var application = new Application
            {
                JobSeekerId = seeker.JobSeekerId,
                JobListingId = jobListingId,
                AppliedDate = DateTime.UtcNow,
                Status = "Pending"
            };

            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            return (true, "Application submitted successfully.");
        }

        // Get applications for an employer
       public async Task<IEnumerable<EmployerApplicationReadDto>> GetApplicationsForEmployerAsync(int userId)
        {
            var employer = await _context.Employees
                .FirstOrDefaultAsync(e => e.UserId == userId);

            if (employer == null) 
                return Enumerable.Empty<EmployerApplicationReadDto>();

            var applications = await _context.Applications
                .Include(a => a.JobListing)
                .Include(a => a.JobSeeker)
                    .ThenInclude(js => js.Resume)
                .Where(a => a.JobListing.EmployerId == employer.EmployerId)
                .Select(a => new EmployerApplicationReadDto
                {
                    ApplicationId = a.ApplicationId,
                    JobListingId = a.JobListingId,
                    JobTitle = a.JobListing.Title,
                    JobSeekerId = a.JobSeekerId,
                    JobSeekerName = a.JobSeeker.FullName,
                    AppliedDate = a.AppliedDate,
                    Status = a.Status,
                    Resume = a.JobSeeker.Resume != null ? new ResumeReadDto
                    {
                        ResumeId = a.JobSeeker.Resume.ResumeId,
                        FilePath = a.JobSeeker.Resume.FilePath,
                        JobSeekerId = a.JobSeeker.Resume.JobSeekerId
                    } : null
                    })
                    .ToListAsync();

            return applications;
        }


        //count of job listings and applications for an employer
        public async Task<JobListingApplicationCountDto> GetJobListingApplicationCountsAsync(int userId)
        {
            var employer = await _context.Employees
                .FirstOrDefaultAsync(e => e.UserId == userId);

            if (employer == null)
            {
                return new JobListingApplicationCountDto
                {
                    TotalJobListings = 0,
                    JobListingsWithApplications = 0
                };
            }

            var totalJobListings = await _context.JobListings
                .CountAsync(j => j.EmployerId == employer.EmployerId);

            var jobListingsWithApplications = await _context.Applications
                .Where(a => a.JobListing.EmployerId == employer.EmployerId)
                .Select(a => a.JobListingId)
                .Distinct()
                .CountAsync();

            return new JobListingApplicationCountDto
            {
                TotalJobListings = totalJobListings,
                JobListingsWithApplications = jobListingsWithApplications
            };
        }

        public async Task<int> GetApplicationCountForJobSeekerAsync(int userId)
        {
            var jobSeeker = await _context.JobSeekers.FirstOrDefaultAsync(js => js.UserId == userId);
            if (jobSeeker == null)
                return 0;

            return await _context.Applications.CountAsync(a => a.JobSeekerId == jobSeeker.JobSeekerId);
        }


    }
}
