using Auth0Maui.UserServices.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth0Maui.UserServices.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AuthenticationItem> Users { get; set; }
    }
}
