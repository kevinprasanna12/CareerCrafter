using CareerCrafter.DTOs.EmployeeDTOs;
using CareerCrafterAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

public class EmployeeControllerTests
{
    private readonly Mock<IEmployeeService> _mockService;
    private readonly EmployeeController _controller;

    public EmployeeControllerTests()
    {
        _mockService = new Mock<IEmployeeService>();
        _controller = new EmployeeController(_mockService.Object);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenEmployeeExists()
    {
        // Arrange
        var employee = new EmployeeReadDto { EmployerId = 1, CompanyName = "TechCo" };
        _mockService.Setup(s => s.GetEmployeeByIdAsync(1)).ReturnsAsync(employee);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedEmployee = Assert.IsType<EmployeeReadDto>(okResult.Value);
        Assert.Equal(1, returnedEmployee.EmployerId);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenEmployeeDoesNotExist()
    {
        _mockService.Setup(s => s.GetEmployeeByIdAsync(1)).ReturnsAsync((EmployeeReadDto)null);

        var result = await _controller.GetById(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenProfileAlreadyExists()
    {
        var createDto = new EmployeeCreateDto { CompanyName = "TechCo", ContactEmail = "hr@techco.com" };
        _mockService.Setup(s => s.GetEmployeeByUserIdAsync(1)).ReturnsAsync(new EmployeeReadDto());

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Role, "Employer")
        }, "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var result = await _controller.Create(createDto);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Profile already exists for this user.", badRequestResult.Value);
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtAction_WhenSuccessful()
    {
        var createDto = new EmployeeCreateDto { CompanyName = "TechCo", ContactEmail = "hr@techco.com" };
        var createdDto = new EmployeeReadDto { EmployerId = 1, CompanyName = "TechCo" };

        _mockService.Setup(s => s.GetEmployeeByUserIdAsync(1)).ReturnsAsync((EmployeeReadDto)null);
        _mockService.Setup(s => s.CreateEmployeeAsync(createDto, 1)).ReturnsAsync(createdDto);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Role, "Employer")
        }, "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var result = await _controller.Create(createDto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnedEmployee = Assert.IsType<EmployeeReadDto>(createdResult.Value);
        Assert.Equal(1, returnedEmployee.EmployerId);
    }
}
