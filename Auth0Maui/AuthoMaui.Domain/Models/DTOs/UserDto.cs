using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthoMaui.Domain.Models.DTOs
{
    internal class UserDto
    {
        public string Id { get; set; } // Assuming this is a Guid in your User entity
        public string Identifier { get; set; }
        public string Email { get; set; }
        public string? Name { get; set; }
        public string? FamilyName { get; set; } // Add this property
        public string? PictureUrl { get; set; } // Add this property
    }
}
