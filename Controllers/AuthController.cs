using absence_tracker.Services;
using absence_tracker.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace absence_tracker.Controllers;

/// <summary>
/// Controller for authentication operations (login, register)
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    /// <summary>
    /// Login endpoint for user authentication
    /// </summary>
    /// <param name="loginRequest">Login request containing username and password</param>
    /// <returns>JWT token if login is successful</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.LoginAsync(loginRequest);

        if (result.Success)
        {
            return Ok(result);
        }

        return Unauthorized(result);
    }

    /// <summary>
    /// Register endpoint for creating a new user account
    /// </summary>
    /// <param name="registerRequest">Registration request containing user details</param>
    /// <returns>Success message if registration is successful</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.RegisterAsync(registerRequest);

        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    /// <summary>
    /// Get user profile endpoint (protected)
    /// </summary>
    /// <returns>User profile information if authenticated</returns>
    [HttpGet("profile")]
    [Authorize]
    public IActionResult GetProfile()
    {
        // Access user information from JWT claims
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var username = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
        var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        var roles = User.FindAll(System.Security.Claims.ClaimTypes.Role).Select(c => c.Value).ToList();

        return Ok(new
        {
            UserId = userId,
            Username = username,
            Email = email,
            Roles = roles,
            Message = "This is a protected endpoint that requires JWT authentication"
        });
    }

    /// <summary>
    /// Refresh token endpoint (optional)
    /// </summary>
    /// <returns>New JWT token</returns>
    [HttpPost("refresh")]
    [Authorize]
    public async Task<IActionResult> RefreshToken()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var result = await _authService.RefreshTokenAsync(userId);

        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}
