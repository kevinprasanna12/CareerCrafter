using CareerCrafter.DTOs.JobListingDTOs;
using CareerCrafterAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Services.Interfaces;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

public class JobListingControllerTests
{
    private readonly Mock<IJobListingService> _mockService;
    private readonly JobListingController _controller;

    public JobListingControllerTests()
    {
        _mockService = new Mock<IJobListingService>();
        _controller = new JobListingController(_mockService.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithJobListings()
    {
        // Arrange
        var jobs = new List<JobListingReadDto> { new JobListingReadDto { JobListingId = 1, Title = "Developer" } };
        _mockService.Setup(s => s.GetAllJobListingsAsync()).ReturnsAsync(jobs);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedJobs = Assert.IsAssignableFrom<IEnumerable<JobListingReadDto>>(okResult.Value);
        Assert.Single(returnedJobs);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenJobExists()
    {
        // Arrange
        var job = new JobListingReadDto { JobListingId = 1, Title = "Developer" };
        _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(job);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedJob = Assert.IsType<JobListingReadDto>(okResult.Value);
        Assert.Equal(1, returnedJob.JobListingId);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenJobDoesNotExist()
    {
        _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((JobListingReadDto)null);

        var result = await _controller.GetById(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtAction()
    {
        // Arrange
        var createDto = new JobListingCreateDto { Title = "Developer", Description = "Backend" };
        var createdDto = new JobListingReadDto { JobListingId = 1, Title = "Developer" };
        _mockService.Setup(s => s.CreateAsync(createDto, It.IsAny<string>())).ReturnsAsync(createdDto);

        var username = "testuser";
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, username)
        }, "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        // Act
        var result = await _controller.Create(createDto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnedJob = Assert.IsType<JobListingReadDto>(createdResult.Value);
        Assert.Equal(1, returnedJob.JobListingId);
    }
}
