using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthoMaui.Domain.Entities.UserManagement
{
    internal class UserPreference
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid PreferenceId { get; set; }
        public Preference Preference { get; set; }
        public bool IsEnabled { get; set; } // Whether the user has this preference enable
    }
}
