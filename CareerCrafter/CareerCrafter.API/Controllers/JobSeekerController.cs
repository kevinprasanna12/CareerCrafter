using CareerCrafter.DTOs.JobSeekerDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Security.Claims;

namespace CareerCrafterAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class JobSeekerController : ControllerBase
    {
        private readonly IJobSeekerService _service;

        public JobSeekerController(IJobSeekerService service)
        {
            _service = service;
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<JobSeekerReadDto>>> GetAll()
        //{
        //    var seekers = await _service.GetAllAsync();
        //    return Ok(seekers);
        //}

        [HttpGet("{id}")]
        public async Task<ActionResult<JobSeekerReadDto>> GetById(int id)
        {
            var seeker = await _service.GetByIdAsync(id);
            return seeker == null ? NotFound() : Ok(seeker);
        }

        [Authorize(Roles = "JobSeeker")]
        [HttpPost]
        public async Task<ActionResult<JobSeekerReadDto>> Create(JobSeekerCreateDto dto)
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);

            var existing = await _service.GetByUserIdAsync(userId);
            if (existing != null)
            {
                return Conflict(new { message = "JobSeeker profile already exists for this user." });
            }
            var created = await _service.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = created.JobSeekerId }, created);
        }

        [Authorize(Roles = "JobSeeker")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, JobSeekerUpdateDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return updated ? NoContent() : NotFound();
        }

        [Authorize(Roles = "JobSeeker")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }

        [Authorize(Roles = "JobSeeker")]
        [HttpGet("my-profile")]
        public async Task<ActionResult<JobSeekerReadDto>> GetMyProfile()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("UserId claim missing in token.");

            int userId = int.Parse(userIdClaim);

            var profile = await _service.GetByUserIdAsync(userId);
            return profile == null ? NotFound() : Ok(profile);
        }
    }
}
