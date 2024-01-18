

using Auth0Maui.Domain.Entities.UserManagement;

namespace Auth0Maui.Domain.Entities.Bases
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? Token { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiryDate;
        public bool IsActive { get; set; }
        public User User { get; set; }
    }
}
