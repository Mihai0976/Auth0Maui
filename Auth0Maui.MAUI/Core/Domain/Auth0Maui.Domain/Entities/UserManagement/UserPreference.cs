using Auth0maui.Domain.Entities.UserManagement;
using System.ComponentModel.DataAnnotations;

namespace ReFeelApp.Common.Domain.Entities.UserManagement;

public class UserPreference
{
    [Key] public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid PreferenceId { get; set; }
    public Preference Preference { get; set; }
    public bool IsEnabled { get; set; } // Whether the user has this preference enabled or not
}