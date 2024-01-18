

namespace Auth0Maui.Domain.Entities.UserManagement
{
    public class UserPreference
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid PreferenceId { get; set; }
        public Preference Preference { get; set; }
        public bool IsEnabled { get; set; } // Whether the user has this preference enable
    }
}
