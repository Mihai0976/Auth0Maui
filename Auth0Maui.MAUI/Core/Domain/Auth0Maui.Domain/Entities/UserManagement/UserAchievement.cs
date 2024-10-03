
namespace ReFeelApp.Common.Domain.Entities.UserManagement;

public class UserAchievement
{
    public Guid UserId { get; set; }
    //public User User { get; set; }
    public Guid AchievementId { get; set; }
   // public Achievement Achievement { get; set; }
    public DateTime DateEarned { get; set; }
}