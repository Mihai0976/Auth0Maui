

using Auth0Maui.Domain.Entities.Bases;
using Auth0Maui.Domain.Entities.UserManagement;

namespace Auth0maui.Domain.Entities.UserManagement;

public class User : AuditableEntity
{
    public UserRole Role { get; set; }
    public string Email { get; set; }
    public string? PasswordHash { get; set; }
    public string? PasswordSalt { get; set; }
    public bool ExternalUser { get; set; }

    public UserProfile UserProfile { get; set; }
}