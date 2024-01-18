using Auth0Maui.Domain.Entities.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth0Maui.Domain.Entities.Helpers
{
    public class AuditLog
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Action { get; set; } // e.g., "Login", "Logout", "PasswordChange"
        public DateTime ActionDate { get; set; }
        public string IPAddress { get; set; }
        public User User { get; set; }
    }
}
