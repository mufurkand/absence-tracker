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

        // TODO: subqueries are used frequently in this service. consider adding a user field to
        // the absence data model or optimize the queries.

        public async Task<ApiResponse<AbsenceDto>> CreateAbsenceAsync(CreateAbsenceDto createAbsenceDto, string userId)
        {
            var response = new ApiResponse<AbsenceDto>();

            try
            {
                // Validate course and user ownership - only check existence, don't load the entity
                var courseExists = await _context.Courses.AnyAsync(c => c.Id == createAbsenceDto.CourseId && c.UserId == userId);

                if (!courseExists)
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

        public async Task<ApiResponse<bool>> DeleteAbsenceAsync(int absenceId, string userId)
        {
            var response = new ApiResponse<bool>();

            try
            {
                // Get the absence if user has permission (single query)
                var absence = await _context.Absences
                    .Where(a => a.Id == absenceId && _context.Courses.Any(c => c.Id == a.CourseId && c.UserId == userId))
                    .FirstOrDefaultAsync();

                if (absence == null)
                {
                    response.Success = false;
                    response.Message = "Absence not found.";
                    return response;
                }

                _context.Absences.Remove(absence);
                await _context.SaveChangesAsync();

                response.Data = true;
                response.Success = true;
                response.Message = "Absence deleted successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error deleting absence: {ex.Message}";
            }

            return response;
        }

        public async Task<ApiResponse<AbsenceDto>> GetAbsenceByIdAsync(int absenceId, string userId)
        {
            var response = new ApiResponse<AbsenceDto>();

            try
            {
                var absence = await _context.Absences
                    .Where(a => a.Id == absenceId && _context.Courses.Any(c => c.Id == a.CourseId && c.UserId == userId))
                    .Select(a => new AbsenceDto
                    {
                        Id = a.Id,
                        CourseId = a.CourseId,
                        Date = a.Date,
                        Reason = a.Reason,
                        CreatedAt = a.CreatedAt,
                        UpdatedAt = a.UpdatedAt
                    }).FirstOrDefaultAsync();

                if (absence == null)
                {
                    response.Success = false;
                    response.Message = "Absence not found.";
                    return response;
                }

                response.Data = absence;
                response.Success = true;
                response.Message = "Absence retrieved successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error retrieving absence: {ex.Message}";
            }

            return response;
        }

        public async Task<ApiResponse<List<AbsenceDto>>> GetAllAbsencesByCourseIdAsync(int courseId, string userId)
        {
            var response = new ApiResponse<List<AbsenceDto>>();

            try
            {
                var course = await _context.Courses
                    .Where(c => c.Id == courseId && c.UserId == userId)
                    .Select(c => new
                    {
                        Absences = c.Absences.Select(a => new AbsenceDto
                        {
                            Id = a.Id,
                            CourseId = a.CourseId,
                            Date = a.Date,
                            Reason = a.Reason,
                            CreatedAt = a.CreatedAt,
                            UpdatedAt = a.UpdatedAt
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();

                if (course == null)
                {
                    response.Success = false;
                    response.Message = "Course not found.";
                    return response;
                }

                response.Data = course.Absences;
                response.Success = true;
                response.Message = "Absences retrieved successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error retrieving absences: {ex.Message}";
            }

            return response;
        }

        public async Task<ApiResponse<AbsenceDto>> UpdateAbsenceAsync(int absenceId, UpdateAbsenceDto updateAbsenceDto, string userId)
        {
            var response = new ApiResponse<AbsenceDto>();

            try
            {
                var absence = await _context.Absences
                        .Where(a => a.Id == absenceId && _context.Courses.Any(c => c.Id == a.CourseId && c.UserId == userId))
                        .FirstOrDefaultAsync();
                if (absence == null)
                {
                    response.Success = false;
                    response.Message = "Absence not found.";
                    return response;
                }

                absence.Date = updateAbsenceDto.Date;
                absence.Reason = updateAbsenceDto.Reason;

                _context.Absences.Update(absence);
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
                response.Message = "Absence updated successfully.";

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error updating absence: {ex.Message}";
            }

            return response;
        }
    }
}