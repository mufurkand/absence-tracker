using absence_tracker.DTOs;

namespace absence_tracker.Services;

/// <summary>
/// Interface for authentication service operations
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Authenticates a user with email and password
    /// </summary>
    /// <param name="loginRequest">Login credentials</param>
    /// <returns>Authentication response with JWT token if successful</returns>
    Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginRequestDto loginRequest);

    /// <summary>
    /// Registers a new user
    /// </summary>
    /// <param name="registerRequest">Registration information</param>
    /// <returns>Authentication response with JWT token if successful</returns>
    Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterRequestDto registerRequest);

    /// <summary>
    /// Refreshes a JWT token (optional implementation for token refresh)
    /// </summary>
    /// <param name="userId">User ID to refresh token for</param>
    /// <returns>New authentication response with refreshed token</returns>
    Task<ApiResponse<AuthResponseDto>> RefreshTokenAsync(string userId);
}
