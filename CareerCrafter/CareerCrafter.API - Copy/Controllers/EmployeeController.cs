using CareerCrafter.DTOs.EmployeeDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace CareerCrafterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;

        public EmployeeController(IEmployeeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeReadDto>>> GetAll()
        {
            var employees = await _service.GetAllEmployeesAsync();
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeReadDto>> GetById(int id)
        {
            var employee = await _service.GetEmployeeByIdAsync(id);
            return employee == null ? NotFound() : Ok(employee);
        }

        [Authorize(Roles = "Employer")]
        [HttpPost]
        public async Task<ActionResult<EmployeeReadDto>> Create(EmployeeCreateDto dto)
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);

            var existing = await _service.GetEmployeeByUserIdAsync(userId);
            if (existing != null)
            {
                return BadRequest("Profile already exists for this user.");
            }

            var created = await _service.CreateEmployeeAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = created.EmployerId }, created);
        }

        [Authorize(Roles = "Employer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, EmployeeUpdateDto dto)
        {
            var updated = await _service.UpdateEmployeeAsync(id, dto);
            return updated ? NoContent() : NotFound();
        }

        [Authorize(Roles = "Employer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteEmployeeAsync(id);
            return deleted ? NoContent() : NotFound();
        }

        [Authorize(Roles = "Employer")]
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<EmployeeReadDto>> GetByUserId(int userId)
        {
            var employee = await _service.GetEmployeeByUserIdAsync(userId);
            if (employee == null)
                return NotFound();
            return Ok(employee);
        }
    }
}
