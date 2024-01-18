

using Auth0Maui.Domain.Entities.UserManagement;

namespace Auth0Maui.Domain.Models.DTOs
{
     public class SwitchModeDto
    {
        public string? UserId { get; set; }
        public Mode NewMode { get; set; }
    }
}

