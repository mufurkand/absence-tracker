using Microsoft.AspNetCore.Mvc;
using absence_tracker.DTOs;

namespace absence_tracker.Controllers
{
    /// <summary>
    /// Base controller providing common functionality for all API controllers
    /// </summary>
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        /// <summary>
        /// Gets the current authenticated user's ID from claims
        /// </summary>
        /// <returns>User ID if authenticated, null otherwise</returns>
        protected string? GetCurrentUserId()
        {
            return User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        }

        /// <summary>
        /// Validates that the user is authenticated and returns an unauthorized response if not
        /// </summary>
        /// <returns>Unauthorized response if user is not authenticated, null if authenticated</returns>
        protected IActionResult? ValidateAuthentication()
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ApiResponse<string>
                {
                    Success = false,
                    Message = "User not authenticated."
                });
            }
            return null;
        }

        /// <summary>
        /// Gets the current user ID with validation, returning an unauthorized response if not authenticated
        /// </summary>
        /// <returns>Tuple containing the user ID and potential unauthorized response</returns>
        protected (string? userId, IActionResult? unauthorizedResponse) GetAuthenticatedUserId()
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return (null, Unauthorized(new ApiResponse<string>
                {
                    Success = false,
                    Message = "User not authenticated."
                }));
            }
            return (userId, null);
        }
    }
}