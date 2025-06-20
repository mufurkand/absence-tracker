using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace absence_tracker.DTOs;

/// <summary>
/// DTO for user login request
/// </summary>
public class LoginDto
{
    [Required]
    [EmailAddress]
    [Description("User's email address")]
    [DefaultValue("john.doe@example.com")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Description("User's password")]
    [DefaultValue("SecurePassword123!")]
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// DTO for user registration request
/// </summary>
public class RegisterDto
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    [Description("User's unique username")]
    [DefaultValue("john_doe")]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [Description("User's email address")]
    [DefaultValue("john.doe@example.com")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6)]
    [Description("User's password")]
    [DefaultValue("SecurePassword123!")]
    public string Password { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    [Description("Password confirmation")]
    [DefaultValue("SecurePassword123!")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

/// <summary>
/// DTO for authentication response containing the JWT token
/// </summary>
public class AuthDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
}
