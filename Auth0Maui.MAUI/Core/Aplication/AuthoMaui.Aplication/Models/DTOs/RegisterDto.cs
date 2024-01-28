
namespace AuthoMaui.Aplication.Models.DTOs;

public class RegisterDto
{
    public Guid UserId { get; set; }
    public string Email { get; set; }

    public string Password { get; set; }
    // other properties as required like name, phone number, etc.
}