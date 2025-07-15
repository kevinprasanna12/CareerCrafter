using CareerCrafter.DTOs.ApplicationDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Security.Claims;

namespace CareerCrafterAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAll() => Ok(await _applicationService.GetAllApplicationsAsync());

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(int id)
        //{
        //    var result = await _applicationService.GetApplicationByIdAsync(id);
        //    return result == null ? NotFound() : Ok(result);
        //}

        //[Authorize(Roles = "Employer")]
        //[HttpPost]
        //public async Task<IActionResult> Create(ApplicationCreateDto dto)
        //{
        //    var created = await _applicationService.CreateApplicationAsync(dto);
        //    return CreatedAtAction(nameof(GetById), new { id = created.ApplicationId }, created);
        //}

        [Authorize(Roles = "JobSeeker")]
        [HttpPost("apply")]
        public async Task<IActionResult> ApplyToJob([FromBody] int jobListingId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier); // typically stores UserId

            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("UserId claim missing in token.");

            int userId = int.Parse(userIdClaim);

            var result = await _applicationService.ApplyToJobAsync(userId, jobListingId);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [Authorize(Roles = "JobSeeker")]
        [HttpGet("my-applications")]
        public async Task<IActionResult> GetMyApplications()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("UserId claim missing in token.");

            int userId = int.Parse(userIdClaim);

            var apps = await _applicationService.GetApplicationsByJobSeekerAsync(userId);
            return Ok(apps);
        }


        [Authorize(Roles = "JobSeeker")]
        [HttpGet("my-applications/count")]
        public async Task<IActionResult> GetMyApplicationCount()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("UserId claim missing in token.");

            int userId = int.Parse(userIdClaim);

            var count = await _applicationService.GetApplicationCountForJobSeekerAsync(userId);
            return Ok(count);
        }


        [Authorize(Roles = "JobSeeker")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _applicationService.DeleteApplicationAsync(id);
            return result ? NoContent() : NotFound();
        }

        [Authorize(Roles = "Employer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ApplicationUpdateDto dto)
        {
            var result = await _applicationService.UpdateApplicationAsync(id, dto);
            return result ? NoContent() : NotFound();
        }

        // ieves applications for the employer
        [Authorize(Roles = "Employer")]
        [HttpGet("my-job-applicants")]
        public async Task<IActionResult> GetMyJobApplicants()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("UserId claim missing in token.");

            int userId = int.Parse(userIdClaim);

            var applicants = await _applicationService.GetApplicationsForEmployerAsync(userId);
            return Ok(applicants);
        }


        // Get counts of applications per job listing for the employer
        [Authorize(Roles = "Employer")]
        [HttpGet("my-job-applicants/count")]
        public async Task<IActionResult> GetJobListingApplicationCounts()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("UserId claim missing in token.");

            int userId = int.Parse(userIdClaim);

            var counts = await _applicationService.GetJobListingApplicationCountsAsync(userId);
            return Ok(counts);
        }

    }
}
