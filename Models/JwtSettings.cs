namespace absence_tracker.Models;

/// <summary>
/// Configuration model for JWT token settings
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// The secret key used to sign JWT tokens
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// The issuer of the JWT token (typically your application name)
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// The intended audience for the JWT token
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Token expiration time in minutes
    /// </summary>
    public int ExpirationInMinutes { get; set; } = 60;
}
