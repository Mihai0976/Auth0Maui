

namespace AuthoMaui.Domain.Models.DTOs;

public class UserDto
{
    public string Id { get; set; } // Assuming this is a Guid in your User entity
    public string Identifier { get; set; }
    public string Email { get; set; }
    public string? Name { get; set; }
    public string? FamilyName { get; set; } // Add this property
    public string? PictureUrl { get; set; } // Add this property
    //public Mode UserMode { get; set; }
}