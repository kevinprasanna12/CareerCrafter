using CareerCrafter.DTOs;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace CareerCrafter.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var result = await _authService.LoginAsync(dto);
            return result == null ? Unauthorized("Invalid credentials.") : Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            var result = await _authService.RegisterAsync(dto);

            // if there's an error key -- return bad request
            if (result is { } && result.GetType().GetProperty("Error") != null)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
