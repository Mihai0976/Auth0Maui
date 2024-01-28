using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthoMaui.Aplication.Models.DTOs;

public class UserLoginModel
{
    public string AccessToken { get; set; }
    public DateTimeOffset AccessTokenExpiration { get; set; }
    public string IdentityToken { get; set; }
    public string RefreshToken { get; set; }
    public bool IsError { get; set; }
    public string Error { get; set; }

    // User information based on claims
    public string Nickname { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public string PictureUrl { get; set; }
    public string UpdatedAt { get; set; }
    public string? UserId { get; set; } // "sub" claim
    public Guid Id { get; set; }
    public string SessionId { get; set; } // "sid" claim

    // Additional fields
    public DateTimeOffset? ExpiresIn { get; set; } // Token expiry duration in seconds
    public string Scope { get; set; }
    public string TokenType { get; set; }
    public string GivenName { get; set; }
    public string FamilyName { get; set; }
    public string Locale { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public long IssuedAt { get; set; }
    public long Expiration { get; set; }
    public string Email { get; set; }
    public bool EmailVerified { get; set; }

    public void PopulateFromClaims(List<Claim> claims)
    {
        // Email
        Email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        // Name-related Claims
        Nickname = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value; // Might need adjustment
        Name = claims.FirstOrDefault(c => c.Type == "name")?.Value; // Custom claim type, might need adjustment
        GivenName = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
        FamilyName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;

        // Locale and Country
        Locale = claims.FirstOrDefault(c => c.Type == ClaimTypes.Locality)?.Value;
        var countryClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Country)?.Value;
        if (!string.IsNullOrWhiteSpace(countryClaim))
            Audience = "+99"; // Assuming '+99' is a placeholder for country code

        // User Identifier
        UserId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var roleClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        

        // Picture URL
        PictureUrl = claims.FirstOrDefault(c => c.Type == "picture")?.Value; // Custom claim type, might need adjustment

        // Email Verified (not available in provided claims, assuming default to false)
        EmailVerified = claims.Any(c => c.Type == "emailverified") &&
                        claims.FirstOrDefault(c => c.Type == "emailverified")?.Value.ToLower() == "true";


        // Issuer and Audience (not available in provided claims, add if necessary)

        // Expiration (assuming 'exp' claim is a Unix timestamp)
        //if (long.TryParse(claims.FirstOrDefault(c => c.Type == "exp")?.Value, out long unixTime))
        //{
        //    Expiration = DateTimeOffset.FromUnixTimeSeconds(unixTime);
        //}

        // Other properties like IssuedAt, SessionId, etc., are not present in the provided claims
        // Populate them if they are available in your claims or required for your application
    }
}
