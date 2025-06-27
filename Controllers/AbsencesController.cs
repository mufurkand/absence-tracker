using absence_tracker.DTOs;
using absence_tracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace absence_tracker.Controllers
{
    [Route("api/[controller]")]
    public class AbsencesController : BaseController
    {
        private readonly IAbsenceService _absenceService;

        public AbsencesController(IAbsenceService absenceService)
        {
            _absenceService = absenceService;
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateAbsence([FromBody] CreateAbsenceDto createAbsenceDto)
        {
            var (userId, unauthorizedResponse) = GetAuthenticatedUserId();
            if (unauthorizedResponse != null) return unauthorizedResponse;

            var response = await _absenceService.CreateAbsenceAsync(createAbsenceDto, userId!);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("delete/{absenceId}")]
        [Authorize]
        public async Task<IActionResult> DeleteAbsence(int absenceId)
        {
            var (userId, unauthorizedResponse) = GetAuthenticatedUserId();
            if (unauthorizedResponse != null) return unauthorizedResponse;

            var response = await _absenceService.DeleteAbsenceAsync(absenceId, userId!);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("course/{courseId}")]
        [Authorize]
        public async Task<IActionResult> GetAllAbsencesByCourseId(int courseId)
        {
            var (userId, unauthorizedResponse) = GetAuthenticatedUserId();
            if (unauthorizedResponse != null) return unauthorizedResponse;

            var response = await _absenceService.GetAllAbsencesByCourseIdAsync(courseId, userId!);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("{absenceId}")]
        [Authorize]
        public async Task<IActionResult> GetAbsenceById(int absenceId)
        {
            var (userId, unauthorizedResponse) = GetAuthenticatedUserId();
            if (unauthorizedResponse != null) return unauthorizedResponse;

            var response = await _absenceService.GetAbsenceByIdAsync(absenceId, userId!);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("update/{absenceId}")]
        [Authorize]
        public async Task<IActionResult> UpdateAbsence(int absenceId, [FromBody] UpdateAbsenceDto updateAbsenceDto)
        {
            var (userId, unauthorizedResponse) = GetAuthenticatedUserId();
            if (unauthorizedResponse != null) return unauthorizedResponse;

            var response = await _absenceService.UpdateAbsenceAsync(absenceId, updateAbsenceDto, userId!);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}