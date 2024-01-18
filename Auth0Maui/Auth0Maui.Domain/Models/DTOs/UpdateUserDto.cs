

namespace Auth0Maui.Domain.Models.DTOs
{
    public class UpdateUserDto
    {
        public Guid? Id { get; set; }
        public Guid? UserId { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }

        public string? CountryPrefix { get; set; }
        // Add other necessary fields
    }
}
