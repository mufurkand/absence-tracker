using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using absence_tracker.Models;

namespace absence_tracker.Services;

/// <summary>
/// Interface for JWT token service operations
/// </summary>
public interface IJwtTokenService
{
    /// <summary>
    /// Generates a JWT token for the specified user
    /// </summary>
    /// <param name="user">The user to generate a token for</param>
    /// <returns>JWT token string</returns>
    Task<string> GenerateTokenAsync(User user);

    /// <summary>
    /// Validates a JWT token and returns the principal
    /// </summary>
    /// <param name="token">The JWT token to validate</param>
    /// <returns>ClaimsPrincipal if valid, null if invalid</returns>
    ClaimsPrincipal? ValidateToken(string token);

    /// <summary>
    /// Gets the token expiration time based on JWT settings
    /// </summary>
    /// <returns>DateTime when tokens expire</returns>
    DateTime GetTokenExpiration();
}
