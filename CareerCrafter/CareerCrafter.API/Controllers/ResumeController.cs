using DAL.Models;
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
        public async Task<ActionResult<IEnumerable<Resume>>> GetAll()
        {
            var resumes = await _service.GetAllAsync();
            return Ok(resumes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Resume>> GetById(int id)
        {
            var resume = await _service.GetByIdAsync(id);
            return resume == null ? NotFound() : Ok(resume);
        }

        [HttpPost]
        public async Task<ActionResult<Resume>> Create(Resume resume)
        {
            var created = await _service.CreateAsync(resume);
            return CreatedAtAction(nameof(GetById), new { id = created.ResumeId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Resume resume)
        {
            var updated = await _service.UpdateAsync(id, resume);
            return updated ? NoContent() : NotFound();
        }

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
