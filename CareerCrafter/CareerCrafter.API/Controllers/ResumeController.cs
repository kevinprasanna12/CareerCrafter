using CareerCrafter.DTOs.ResumeDTOs;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using System.Security.Claims;

namespace CareerCrafterAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ResumeController : ControllerBase
    {
        private readonly IResumeService _service;
        private readonly CareerCrafterDbContext _context;

        public ResumeController(IResumeService service,CareerCrafterDbContext context)
        {
            _service = service;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResumeReadDto>>> GetAll()
        {
            var resumes = await _service.GetAllAsync();
            return Ok(resumes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResumeReadDto>> GetById(int id)
        {
            var resume = await _service.GetByIdAsync(id);
            return resume == null ? NotFound() : Ok(resume);
        }

        //[Authorize(Roles = "JobSeeker")]
        //[HttpPost]
        //public async Task<ActionResult<ResumeReadDto>> Create(ResumeCreateDto dto)
        //{
        //    var created = await _service.CreateAsync(dto);
        //    return CreatedAtAction(nameof(GetById), new { id = created.ResumeId }, created);
        //}

        [Authorize(Roles = "JobSeeker")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ResumeUpdateDto dto)
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
        [HttpPost("upload-resume")]
        public async Task<IActionResult> UploadResume(IFormFile file)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("UserId claim missing in token.");

            int userId = int.Parse(userIdClaim);

            var path = await _service.UploadResumeAsync(file, userId);
            return path == null ? BadRequest("Upload failed.") : Ok("Resume uploaded successfully.");
        }

        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadResume(int id)
        {
            var result = await _service.DownloadResumeAsync(id);
            if (result == null)
            {
                return NotFound("Resume not found.");
            }

            return File(result.Value.FileBytes, result.Value.ContentType, result.Value.FileName);
        }

        [HttpGet("my-resume-id")]
        [Authorize(Roles = "JobSeeker")]
        public async Task<ActionResult<int>> GetMyResumeId()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized("UserId claim not found in token.");

                if (!int.TryParse(userIdClaim, out int userId))
                    return BadRequest("Invalid UserId claim value.");

                var jobSeeker = await _context.JobSeekers
                    .FirstOrDefaultAsync(js => js.UserId == userId);

                if (jobSeeker == null)
                    return NotFound("JobSeeker profile not found.");

                var resume = await _context.Resumes
                    .FirstOrDefaultAsync(r => r.JobSeekerId == jobSeeker.JobSeekerId);

                if (resume == null)
                    return NotFound("Resume not found for this JobSeeker.");

                return Ok(resume.ResumeId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



    }
}
