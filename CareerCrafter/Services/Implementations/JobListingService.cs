using CareerCrafter.DTOs.JobListingDTOs;
using DAL;
using DAL.Models;
using DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services.Implementations
{
    public class JobListingService : IJobListingService
    {
        private readonly IJobListingRepository _repository;
        private readonly CareerCrafterDbContext _context;

        public JobListingService(IJobListingRepository repository, CareerCrafterDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<IEnumerable<JobListingReadDto>> GetAllAsync()
        {
            var jobs = await _repository.GetAllAsync();
            return jobs.Select(j => new JobListingReadDto
            {
                JobListingId = j.JobListingId,
                Title = j.Title,
                Description = j.Description,
                Location = j.Location,
                Qualifications = j.Qualifications,
                EmployerId = j.EmployerId
            });
        }

        public async Task<IEnumerable<JobListingReadDto>> GetAllJobListingsAsync()
        {
            return await _context.JobListings
                .Include(j => j.Employer)
                .Select(j => new JobListingReadDto
                {
                    JobListingId = j.JobListingId,
                    Title = j.Title,
                    Description = j.Description,
                    Location = j.Location,
                    Qualifications = j.Qualifications,
                    Salary = j.Salary,
                    EmployerId = j.EmployerId,
                    CompanyName = j.Employer.CompanyName
                })
                .ToListAsync();
        }


        public async Task<JobListingReadDto?> GetByIdAsync(int id)
        {
            var j = await _repository.GetByIdAsync(id);
            return j == null ? null : new JobListingReadDto
            {
                JobListingId = j.JobListingId,
                Title = j.Title,
                Description = j.Description,
                Location = j.Location,
                Qualifications = j.Qualifications,
                EmployerId = j.EmployerId
            };
        }

        public async Task<JobListingReadDto> CreateAsync(JobListingCreateDto dto, string username)
        {
            // Get the User
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.Role == "Employer");

            if (user == null)
                throw new Exception("Unauthorized to create job listings.");

            // Fetch the corresponding Employee using UserId
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.UserId == user.UserId);

            if (employee == null)
                throw new Exception("Complete your employer profile before creating job listings.");

            var entity = new JobListing
            {
                Title = dto.Title,
                Description = dto.Description,
                Location = dto.Location,
                Qualifications = dto.Qualifications,
                Salary = dto.Salary, // Optional field, can be null
                EmployerId = employee.EmployerId ,
            };

            var created = await _repository.AddAsync(entity);

            return new JobListingReadDto
            {
                JobListingId = created.JobListingId,
                Title = created.Title,
                Description = created.Description,
                Location = created.Location,
                Qualifications = created.Qualifications,
                Salary = created.Salary,
                EmployerId = created.EmployerId
            };
        }

        public async Task<bool> UpdateAsync(int id, JobListingUpdateDto dto, int userId)
        {
            var jobListing = await _repository.GetByIdAsync(id);
            if (jobListing == null)
                return false; // Not found

            // Fetch Employer associated with this JobListing
            var employer = await _context.Employees.FindAsync(jobListing.EmployerId);
            if (employer == null)
                return false; // Employer data corrupted

            // Check ownership
            if (employer.UserId != userId)
                return false; // Forbidden

            // Authorized - perform update
            jobListing.Title = dto.Title;
            jobListing.Description = dto.Description;
            jobListing.Location = dto.Location;
            jobListing.Qualifications = dto.Qualifications;
            jobListing.Salary = dto.Salary; // Optional field, can be null

            await _repository.UpdateAsync(jobListing);
            return true;
        }



        public async Task<bool> DeleteAsync(int id, string username)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.Role == "Employer");

            if (user == null)
                return false;

            var employer = await _context.Employees
                .FirstOrDefaultAsync(e => e.UserId == user.UserId);

            if (employer == null)
                return false;

            var job = await _repository.GetByIdAsync(id);
            if (job == null || job.EmployerId != employer.EmployerId)
                return false;

            await _repository.DeleteAsync(id);
            return true;
        }


        public async Task<IEnumerable<JobListingReadDto>> GetMyJobsAsync(int userId)
        {
            // Fetch the Employee record for this userId
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.UserId == userId);

            if (employee == null)
                return Enumerable.Empty<JobListingReadDto>();

            // Fetch JobListings where EmployeeId matches the EmployerId of this employee
            var jobs = await _context.JobListings
                .Where(j => j.EmployerId == employee.EmployerId)
                .ToListAsync();

            return jobs.Select(j => new JobListingReadDto
            {
                JobListingId = j.JobListingId,
                Title = j.Title,
                Description = j.Description,
                Location = j.Location,
                Qualifications = j.Qualifications,
                Salary = j.Salary,
                EmployerId = j.EmployerId,
                CompanyName = employee.CompanyName   

            });
        }

    }
}
