using absence_tracker.Services;
using absence_tracker.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace absence_tracker.Controllers;

/// <summary>
/// Controller for authentication operations (login, register)
/// This is a simple example controller to demonstrate JWT authentication
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Login endpoint that returns JWT token
    /// </summary>
    /// <param name="loginRequest">Login credentials</param>
    /// <returns>JWT token if authentication is successful</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
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
    /// Register endpoint that creates a new user and returns JWT token
    /// </summary>
    /// <param name="registerRequest">Registration information</param>
    /// <returns>JWT token if registration is successful</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequest)
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
    /// Protected endpoint that requires JWT authentication
    /// This demonstrates how to protect routes with JWT
    /// </summary>
    /// <returns>User information from JWT claims</returns>
    [HttpGet("profile")]
    [Authorize] // This attribute requires a valid JWT token
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
