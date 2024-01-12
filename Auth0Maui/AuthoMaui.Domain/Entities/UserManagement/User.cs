using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthoMaui.Domain.Entities.UserManagement
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
