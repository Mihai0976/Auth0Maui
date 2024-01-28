using System.ComponentModel.DataAnnotations;
using MediatR;
using Auth0Maui.Domain.Models.Results;
using AuthoMaui.Domain.Models.DTOs;

namespace Auth0Maui.Domain.Models.Commands.UserManagement.Auth;

public class RegisterUserCommand : IRequest<CommandResult<RegisterDto>>
{
    [Required] public string Email { get; set; }

    // Password should be handled carefully, especially if coming from external identity providers
    public string? Password { get; set; }

    // Claims related properties
    public string? Nickname { get; set; }
    public string? Name { get; set; }
    public string? PictureUrl { get; set; }
    public string? Locale { get; set; }
    public string? GivenName { get; set; }
    public string? FamilyName { get; set; }
    public bool? EmailVerified { get; set; }

    // ExternalInfo related properties
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public string? SessionId { get; set; }
    public string? ExternalIdentifier { get; set; }
    public long? IssuedAt { get; set; }
    public long? Expiration { get; set; }

    // Token related properties
    public string? AccessToken { get; set; }
    public DateTimeOffset? AccessTokenExpiration { get; set; }
    public string? IdentityToken { get; set; }
    public string? RefreshToken { get; set; }
    public long? ExpiresIn { get; set; } // Token expiry duration in seconds
    public string? Scope { get; set; }
    public string? TokenType { get; set; }
    public bool? IsAuthenticated { get; set; } // This should be set based on ClaimsIdentity.IsAuthenticated
}