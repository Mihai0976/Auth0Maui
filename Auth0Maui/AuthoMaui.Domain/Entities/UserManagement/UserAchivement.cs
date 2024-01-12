using AuthoMaui.Domain.Entities.Achivements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthoMaui.Domain.Entities.UserManagement
{
    internal class UserAchivement
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid AchievementId { get; set; }
        public Achievement Achievement { get; set; }
        public DateTime DateEarned { get; set; }
    }
}
