using CareerCrafter.DTOs.JobListingDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Security.Claims;

namespace CareerCrafterAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class JobListingController : ControllerBase
    {
        private readonly IJobListingService _service;

        public JobListingController(IJobListingService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobListingReadDto>>> GetAll()
        {
            var jobs = await _service.GetAllJobListingsAsync();
            return Ok(jobs);
        }

        [Authorize(Roles = "Employer")] 
        
        [HttpGet("{id}")]
        public async Task<ActionResult<JobListingReadDto>> GetById(int id)
        {
            var job = await _service.GetByIdAsync(id);
            return job == null ? NotFound() : Ok(job);
        }


        [Authorize(Roles = "Employer")]
        [ProducesResponseType(typeof(JobListingReadDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<JobListingReadDto>> Create(JobListingCreateDto dto)
        {
            var username = User.Identity?.Name ?? string.Empty;
            var created = await _service.CreateAsync(dto, username);
            return CreatedAtAction(nameof(GetById), new { id = created.JobListingId }, created);
        }



        [Authorize(Roles = "Employer")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, JobListingUpdateDto dto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("UserId claim missing in token.");

            int userId = int.Parse(userIdClaim);

            var updated = await _service.UpdateAsync(id, dto,userId);

            if (!updated)
            {
                // Decide whether to return NotFound or Forbid:
                // If your service returns false for both "not found" and "forbidden",
                // you may initially return NotFound, or:
                return Forbid(); // or return NotFound();
            }

            Console.WriteLine("Update succeeded."); // Debug
            return NoContent();
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
        public async Task<ActionResult<IEnumerable<JobListingReadDto>>> GetMyJobs()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid UserId in token.");

            var jobs = await _service.GetMyJobsAsync(userId);
            return Ok(jobs);
        }
    }
}
