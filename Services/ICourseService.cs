using absence_tracker.DTOs;

namespace absence_tracker.Services
{
    public interface ICourseService
    {
        Task<ApiResponse<List<CourseDto>>> GetAllCoursesByUserIdAsync(string userId);
        Task<ApiResponse<CourseDto>> GetCourseByIdAsync(int courseId, string userId);
        Task<ApiResponse<CourseDto>> CreateCourseAsync(CreateCourseDto createCourseDto, string userId);
        Task<ApiResponse<CourseDto>> UpdateCourseAsync(int courseId, UpdateCourseDto updateCourseDto, string userId);
        Task<ApiResponse<bool>> DeleteCourseAsync(int courseId, string userId);
    }
}