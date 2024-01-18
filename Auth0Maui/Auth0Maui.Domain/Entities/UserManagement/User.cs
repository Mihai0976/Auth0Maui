
namespace Auth0Maui.Domain.Entities.UserManagement
{
    public class User
    {
        public UserRole Role { get; set; }
        public string Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }
        public bool ExternalUser { get; set; }

        public UserProfile UserProfile { get; set; }
    }
}
