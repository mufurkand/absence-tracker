using absence_tracker.DTOs;
using absence_tracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace absence_tracker.Controllers
{
    /// <summary>
    /// Controller for course-related operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;
        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDto courseRegisterDto)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ApiResponse<string> { Success = false, Message = "User not authenticated." });
            }

            var response = await _courseService.CreateCourseAsync(courseRegisterDto, userId);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}