using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace CareerCrafterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobSeekerController : ControllerBase
    {
        private readonly IJobSeekerService _service;

        public JobSeekerController(IJobSeekerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobSeeker>>> GetAll()
        {
            var seekers = await _service.GetAllAsync();
            return Ok(seekers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JobSeeker>> GetById(int id)
        {
            var seeker = await _service.GetByIdAsync(id);
            return seeker == null ? NotFound() : Ok(seeker);
        }

        [HttpPost]
        public async Task<ActionResult<JobSeeker>> Create(JobSeeker jobSeeker)
        {
            var created = await _service.CreateAsync(jobSeeker);
            return CreatedAtAction(nameof(GetById), new { id = created.JobSeekerId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, JobSeeker jobSeeker)
        {
            var updated = await _service.UpdateAsync(id, jobSeeker);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
