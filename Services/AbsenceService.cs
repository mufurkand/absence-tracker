using absence_tracker.Data;
using absence_tracker.DTOs;
using absence_tracker.Models;
using Microsoft.EntityFrameworkCore;

namespace absence_tracker.Services
{
    public class AbsenceService : IAbsenceService
    {
        private readonly AbsenceTrackerDbContext _context;
        public AbsenceService(AbsenceTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<AbsenceDto>> CreateAbsenceAsync(CreateAbsenceDto createAbsenceDto, string userId)
        {
            var response = new ApiResponse<AbsenceDto>();

            try
            {
                // Validate course and user ownership
                var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == createAbsenceDto.CourseId && c.UserId == userId);

                if (course == null)
                {
                    response.Success = false;
                    response.Message = "Course not found or you don't have permission to create absence for this course.";
                    return response;
                }

                var absence = new Absence
                {
                    CourseId = createAbsenceDto.CourseId,
                    Date = createAbsenceDto.Date,
                    Reason = createAbsenceDto.Reason
                };

                _context.Absences.Add(absence);
                await _context.SaveChangesAsync();

                var absenceDto = new AbsenceDto
                {
                    Id = absence.Id,
                    CourseId = absence.CourseId,
                    Date = absence.Date,
                    Reason = absence.Reason,
                    CreatedAt = absence.CreatedAt,
                    UpdatedAt = absence.UpdatedAt
                };

                response.Data = absenceDto;
                response.Success = true;
                response.Message = "Absence created successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error creating absence: {ex.Message}";
            }

            return response;
        }
    }
}