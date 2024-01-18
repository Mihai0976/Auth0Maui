using Auth0Maui.Domain.Entities.Achivements;


namespace Auth0Maui.Domain.Entities.UserManagement
{
    public class UserAchivement
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid AchievementId { get; set; }
        public Achievement Achievement { get; set; }
        public DateTime DateEarned { get; set; }
    }
}
