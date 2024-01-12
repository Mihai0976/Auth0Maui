using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthoMaui.Domain.Models.DTOs
{
    internal class UpdateUserDto
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
