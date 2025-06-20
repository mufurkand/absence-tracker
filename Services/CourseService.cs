using absence_tracker.Data;
using absence_tracker.DTOs;
using absence_tracker.Models;

namespace absence_tracker.Services
{
    public class CourseService : ICourseService
    {
        private readonly AbsenceTrackerDbContext _context;
        public CourseService(AbsenceTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<CourseDto>> CreateCourseAsync(CreateCourseDto courseRegisterDto, string userId)
        {
            var response = new ApiResponse<CourseDto>();

            try
            {
                var course = new Course
                {
                    Name = courseRegisterDto.Name,
                    Description = courseRegisterDto.Description,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
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

        public Task<ApiResponse<bool>> DeleteCourseAsync(int courseId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<List<CourseDto>>> GetAllCoursesAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<CourseDto>> GetCourseByIdAsync(int courseId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<CourseDto>> UpdateCourseAsync(int courseId, UpdateCourseDto courseUpdateDto, string userId)
        {
            throw new NotImplementedException();
        }
    }
}