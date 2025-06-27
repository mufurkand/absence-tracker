using absence_tracker.DTOs;

namespace absence_tracker.Services
{
    public interface IAbsenceService
    {
        Task<ApiResponse<AbsenceDto>> CreateAbsenceAsync(CreateAbsenceDto createAbsenceDto, string userId);
        Task<ApiResponse<AbsenceDto>> GetAbsenceByIdAsync(int absenceId, string userId);
        Task<ApiResponse<List<AbsenceDto>>> GetAllAbsencesByCourseIdAsync(int courseId, string userId);
        Task<ApiResponse<AbsenceDto>> UpdateAbsenceAsync(int absenceId, UpdateAbsenceDto updateAbsenceDto, string userId);
        Task<ApiResponse<bool>> DeleteAbsenceAsync(int absenceId, string userId);
    }
}