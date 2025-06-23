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

        [HttpPost]
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
    }
}