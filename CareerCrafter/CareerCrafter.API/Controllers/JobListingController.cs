using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Security.Claims;

namespace CareerCrafterAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class JobListingController : ControllerBase
    {
        private readonly IJobListingService _service;

        public JobListingController(IJobListingService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobListing>>> GetAll()
        {
            var jobs = await _service.GetAllAsync();
            return Ok(jobs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JobListing>> GetById(int id)
        {
            var job = await _service.GetByIdAsync(id);
            return job == null ? NotFound() : Ok(job);
        }

        [Authorize(Roles = "Employer")]
        [HttpPost]
        public async Task<ActionResult<JobListing>> Create(JobListing jobListing)
        {
            var created = await _service.CreateAsync(jobListing);
            return CreatedAtAction(nameof(GetById), new { id = created.JobListingId }, created);
        }

        [Authorize(Roles = "Employer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, JobListing jobListing)
        {
            var username = User.Identity?.Name ?? string.Empty;
            var updated = await _service.UpdateAsync(id, jobListing, username);
            return updated ? NoContent() : Forbid();
        }

        [Authorize(Roles = "Employer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var username = User.Identity?.Name ?? string.Empty;
            var deleted = await _service.DeleteAsync(id, username);
            return deleted ? NoContent() : Forbid();
        }

        [Authorize(Roles = "Employer")]
        [HttpGet("my-jobs")]
        public async Task<IActionResult> GetMyJobs()
        {
            var username = User.Identity?.Name ?? string.Empty;
            var jobs = await _service.GetMyJobsAsync(username);
            return Ok(jobs);
        }
    }
}
