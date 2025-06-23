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
    public class CoursesController : BaseController
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
            var (userId, unauthorizedResponse) = GetAuthenticatedUserId();
            if (unauthorizedResponse != null) return unauthorizedResponse;

            var response = await _courseService.CreateCourseAsync(courseRegisterDto, userId!);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("delete/{courseId}")]
        [Authorize]
        public async Task<IActionResult> DeleteCourse(int courseId)
        {
            var (userId, unauthorizedResponse) = GetAuthenticatedUserId();
            if (unauthorizedResponse != null) return unauthorizedResponse;

            var response = await _courseService.DeleteCourseAsync(courseId, userId!);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("all")]
        [Authorize]
        public async Task<IActionResult> GetAllCourses()
        {
            var (userId, unauthorizedResponse) = GetAuthenticatedUserId();
            if (unauthorizedResponse != null) return unauthorizedResponse;

            var response = await _courseService.GetAllCoursesByUserIdAsync(userId!);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("{courseId}")]
        [Authorize]
        public async Task<IActionResult> GetCourseById(int courseId)
        {
            var (userId, unauthorizedResponse) = GetAuthenticatedUserId();
            if (unauthorizedResponse != null) return unauthorizedResponse;

            var response = await _courseService.GetCourseByIdAsync(courseId, userId!);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("update/{courseId}")]
        [Authorize]
        public async Task<IActionResult> UpdateCourse(int courseId, [FromBody] UpdateCourseDto updateCourseDto)
        {
            var (userId, unauthorizedResponse) = GetAuthenticatedUserId();
            if (unauthorizedResponse != null) return unauthorizedResponse;

            var response = await _courseService.UpdateCourseAsync(courseId, updateCourseDto, userId!);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}