

using Auth0maui.Domain.Entities.UserManagement;
using Auth0Maui.Domain.Entities.Bases;

namespace Auth0Maui.Domain.Entities.UserManagement;

public class UserProfile : AuditableEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; }

    // User information based on claims
    public string? Nickname { get; set; }
    public string? Name { get; set; }
    public string? GivenName { get; set; }
    public string? FamilyName { get; set; }
    public string? Address { get; set; }
    public string? PictureUrl { get; set; }
    public DateTime UpdatedAt { get; set; }

    public bool EmailVerified { get; set; }
    public string? Locale { get; set; }

    public long Expiration { get; set; }
    public ExternalInfo ExternalInfo { get; set; }
    public string? PhoneNumber { get; set; }
    public string? CountryPrefix { get; set; }
}