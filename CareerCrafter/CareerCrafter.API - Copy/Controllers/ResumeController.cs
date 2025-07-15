using CareerCrafter.DTOs.ResumeDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Security.Claims;

namespace CareerCrafterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResumeController : ControllerBase
    {
        private readonly IResumeService _service;

        public ResumeController(IResumeService service)
        {
            _service = service;
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

        [Authorize(Roles = "JobSeeker")]
        [HttpPost]
        public async Task<ActionResult<ResumeReadDto>> Create(ResumeCreateDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.ResumeId }, created);
        }

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
            var email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
            var path = await _service.UploadResumeAsync(file, email);
            return path == null ? BadRequest("Upload failed.") : Ok("Resume uploaded successfully.");
        }
    }
}
