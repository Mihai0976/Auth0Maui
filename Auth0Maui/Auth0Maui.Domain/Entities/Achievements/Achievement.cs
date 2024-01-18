using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth0Maui.Domain.Entities.Achivements
{
    public class Achievement
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? BadgeImageUrl { get; set; }
    }
}
