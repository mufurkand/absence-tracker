using absence_tracker.Data;
using absence_tracker.DTOs;
using absence_tracker.Models;
using Microsoft.EntityFrameworkCore;

namespace absence_tracker.Services
{
    public class CourseService : ICourseService
    {
        private readonly AbsenceTrackerDbContext _context;
        public CourseService(AbsenceTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<CourseDto>> CreateCourseAsync(CreateCourseDto createCourseDto, string userId)
        {
            var response = new ApiResponse<CourseDto>();

            try
            {
                var course = new Course
                {
                    Name = createCourseDto.Name,
                    Description = createCourseDto.Description,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Courses.Add(course);
                await _context.SaveChangesAsync();

                var courseDto = new CourseDto
                {
                    Id = course.Id,
                    Name = course.Name,
                    Description = course.Description,
                    CreatedAt = course.CreatedAt
                };

                response.Data = courseDto;
                response.Success = true;
                response.Message = "Course created successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error creating course: {ex.Message}";
            }

            return response;
        }

        public async Task<ApiResponse<bool>> DeleteCourseAsync(int courseId, string userId)
        {
            var response = new ApiResponse<bool>();
            try
            {
                var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId && c.UserId == userId);
                if (course == null)
                {
                    response.Success = false;
                    response.Message = "Course not found.";
                    return response;
                }

                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();

                response.Data = true;
                response.Success = true;
                response.Message = "Course deleted successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error deleting course: {ex.Message}";
            }

            return response;
        }

        public async Task<ApiResponse<List<CourseDto>>> GetAllCoursesByUserIdAsync(string userId)
        {
            var response = new ApiResponse<List<CourseDto>>();

            try
            {
                var courses = await _context.Courses
                    .Where(c => c.UserId == userId)
                    .Select(c => new CourseDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        TotalAbsences = c.Absences.Count(),
                        CreatedAt = c.CreatedAt,
                        UpdatedAt = c.UpdatedAt
                    })
                    .ToListAsync();

                response.Data = courses;
                response.Success = true;
                response.Message = "Courses retrieved successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error retrieving courses: {ex.Message}";
            }

            return response;
        }

        public async Task<ApiResponse<CourseDto>> GetCourseByIdAsync(int courseId, string userId)
        {
            var response = new ApiResponse<CourseDto>();

            try
            {
                var course = await _context.Courses
                    .Where(c => c.Id == courseId && c.UserId == userId)
                    .Select(c => new CourseDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        TotalAbsences = c.Absences.Count(),
                        CreatedAt = c.CreatedAt,
                        UpdatedAt = c.UpdatedAt
                    })
                    .FirstOrDefaultAsync();

                if (course == null)
                {
                    response.Success = false;
                    response.Message = "Course not found.";
                    return response;
                }

                response.Data = course;
                response.Success = true;
                response.Message = "Course retrieved successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error retrieving course: {ex.Message}";
            }

            return response;
        }

        public async Task<ApiResponse<CourseDto>> UpdateCourseAsync(int courseId, UpdateCourseDto updateCourseDto, string userId)
        {
            var response = new ApiResponse<CourseDto>();

            try
            {
                var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId && c.UserId == userId);
                if (course == null)
                {
                    response.Success = false;
                    response.Message = "Course not found.";
                    return response;
                }

                // Update properties
                course.Name = updateCourseDto.Name;
                course.Description = updateCourseDto.Description;
                course.UpdatedAt = DateTime.UtcNow;

                _context.Courses.Update(course);
                await _context.SaveChangesAsync();

                var courseDto = new CourseDto
                {
                    Id = course.Id,
                    Name = course.Name,
                    Description = course.Description,
                    TotalAbsences = course.Absences?.Count() ?? 0,
                    CreatedAt = course.CreatedAt,
                    UpdatedAt = course.UpdatedAt
                };

                response.Data = courseDto;
                response.Success = true;
                response.Message = "Course updated successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error updating course: {ex.Message}";
            }

            return response;
        }
    }
}