using absence_tracker.DTOs;

namespace absence_tracker.Services
{
    public interface IAbsenceService
    {
        Task<ApiResponse<AbsenceDto>> CreateAbsenceAsync(CreateAbsenceDto createAbsenceDto, string userId);
    }
}