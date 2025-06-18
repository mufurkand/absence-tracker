using Microsoft.AspNetCore.Identity;
using absence_tracker.Models;
using absence_tracker.DTOs;

namespace absence_tracker.Services;

/// <summary>
/// Service for handling user authentication operations
/// </summary>
public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IJwtTokenService jwtTokenService,
        ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenService = jwtTokenService;
        _logger = logger;
    }

    /// <summary>
    /// Authenticates a user and returns a JWT token
    /// </summary>
    public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginRequestDto loginRequest)
    {
        try
        {
            // Find user by email
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user == null)
            {
                _logger.LogWarning("Login attempt failed: User not found for email {Email}", loginRequest.Email);
                return ApiResponse<AuthResponseDto>.ErrorResponse("Invalid email or password.");
            }

            // Check password
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, lockoutOnFailure: true);

            if (!result.Succeeded)
            {
                var errorMessage = result.IsLockedOut
                    ? "Account is locked due to multiple failed login attempts."
                    : "Invalid email or password.";

                _logger.LogWarning("Login attempt failed for user {UserId}: {Reason}",
                    user.Id, result.IsLockedOut ? "Account locked" : "Invalid password");

                return ApiResponse<AuthResponseDto>.ErrorResponse(errorMessage);
            }

            // Generate JWT token
            var token = await _jwtTokenService.GenerateTokenAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var authResponse = new AuthResponseDto
            {
                Token = token,
                Expiration = _jwtTokenService.GetTokenExpiration(),
                UserId = user.Id,
                Username = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Roles = roles.ToList()
            };

            _logger.LogInformation("User {UserId} successfully logged in", user.Id);
            return ApiResponse<AuthResponseDto>.SuccessResponse(authResponse, "Login successful.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login attempt for email {Email}", loginRequest.Email);
            return ApiResponse<AuthResponseDto>.ErrorResponse("An error occurred during login.");
        }
    }

    /// <summary>
    /// Registers a new user and returns a JWT token
    /// </summary>
    public async Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterRequestDto registerRequest)
    {
        try
        {
            // Check if user already exists
            var existingUser = await _userManager.FindByEmailAsync(registerRequest.Email);
            if (existingUser != null)
            {
                return ApiResponse<AuthResponseDto>.ErrorResponse("A user with this email already exists.");
            }

            existingUser = await _userManager.FindByNameAsync(registerRequest.Username);
            if (existingUser != null)
            {
                return ApiResponse<AuthResponseDto>.ErrorResponse("A user with this username already exists.");
            }

            // Create new user
            var user = new User
            {
                UserName = registerRequest.Username,
                Email = registerRequest.Email,
                EmailConfirmed = false // You might want to implement email confirmation
            };

            var result = await _userManager.CreateAsync(user, registerRequest.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                _logger.LogWarning("User registration failed for email {Email}: {Errors}",
                    registerRequest.Email, string.Join(", ", errors));

                return ApiResponse<AuthResponseDto>.ErrorResponse("Registration failed.", errors);
            }

            // Generate JWT token for the new user
            var token = await _jwtTokenService.GenerateTokenAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var authResponse = new AuthResponseDto
            {
                Token = token,
                Expiration = _jwtTokenService.GetTokenExpiration(),
                UserId = user.Id,
                Username = user.UserName,
                Email = user.Email ?? string.Empty,
                Roles = roles.ToList()
            };

            _logger.LogInformation("User {UserId} successfully registered", user.Id);
            return ApiResponse<AuthResponseDto>.SuccessResponse(authResponse, "Registration successful.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for email {Email}", registerRequest.Email);
            return ApiResponse<AuthResponseDto>.ErrorResponse("An error occurred during registration.");
        }
    }

    /// <summary>
    /// Refreshes a JWT token for an existing user
    /// </summary>
    public async Task<ApiResponse<AuthResponseDto>> RefreshTokenAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return ApiResponse<AuthResponseDto>.ErrorResponse("User not found.");
            }

            // Generate new JWT token
            var token = await _jwtTokenService.GenerateTokenAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var authResponse = new AuthResponseDto
            {
                Token = token,
                Expiration = _jwtTokenService.GetTokenExpiration(),
                UserId = user.Id,
                Username = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Roles = roles.ToList()
            };

            _logger.LogInformation("Token refreshed for user {UserId}", user.Id);
            return ApiResponse<AuthResponseDto>.SuccessResponse(authResponse, "Token refreshed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error refreshing token for user {UserId}", userId);
            return ApiResponse<AuthResponseDto>.ErrorResponse("An error occurred during token refresh.");
        }
    }
}
