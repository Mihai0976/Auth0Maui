namespace AuthoMaui.Aplication.Models.DTOs;

public class UserRegistrationModel
{
    public string? Email { get; set; }
    public string? Password { get; set; }

    public string? AccessToken { get; set; }
    public DateTimeOffset? AccessTokenExpiration { get; set; }
    public string? IdentityToken { get; set; }
    public string? RefreshToken { get; set; }
    public bool? IsError { get; set; }
    public string? Error { get; set; }

    // User information based on claims
    public string Nickname { get; set; }
    public string Name { get; set; }
    public string PictureUrl { get; set; }
    public string? UpdatedAt { get; set; }
    public string? UserId { get; set; } // "sub" claim
    public string? SessionId { get; set; } // "sid" claim

    // Additional fields
    public DateTimeOffset? ExpiresIn { get; set; } // Token expiry duration in seconds
    public string? Scope { get; set; }
    public string? TokenType { get; set; }
    public string? GivenName { get; set; }
    public string FamilyName { get; set; }
    public string? Locale { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public long? IssuedAt { get; set; }
    public long? Expiration { get; set; }
    public bool? EmailVerified { get; set; }
}