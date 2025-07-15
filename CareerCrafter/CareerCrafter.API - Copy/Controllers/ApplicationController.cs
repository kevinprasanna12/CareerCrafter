using CareerCrafter.DTOs.ApplicationDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Security.Claims;

namespace CareerCrafterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _applicationService.GetAllApplicationsAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _applicationService.GetApplicationByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [Authorize(Roles = "Employer")]
        [HttpPost]
        public async Task<IActionResult> Create(ApplicationCreateDto dto)
        {
            var created = await _applicationService.CreateApplicationAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.ApplicationId }, created);
        }

        [Authorize(Roles = "Employer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ApplicationUpdateDto dto)
        {
            var result = await _applicationService.UpdateApplicationAsync(id, dto);
            return result ? NoContent() : NotFound();
        }

        [Authorize(Roles = "Employer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _applicationService.DeleteApplicationAsync(id);
            return result ? NoContent() : NotFound();
        }

        [Authorize(Roles = "JobSeeker")]
        [HttpGet("my-applications")]
        public async Task<IActionResult> GetMyApplications()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var apps = await _applicationService.GetApplicationsByJobSeekerAsync(email);
            return Ok(apps);
        }

        [Authorize(Roles = "JobSeeker")]
        [HttpPost("apply")]
        public async Task<IActionResult> ApplyToJob([FromBody] int jobListingId)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await _applicationService.ApplyToJobAsync(email, jobListingId);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }
    }
}
