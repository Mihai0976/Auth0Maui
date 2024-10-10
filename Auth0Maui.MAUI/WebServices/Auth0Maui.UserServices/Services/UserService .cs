using Auth0Maui.UserServices.Data;
using Auth0Maui.UserServices.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Auth0Maui.UserServices.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        // This method retrieves the user by their ID
        public AuthenticationItem GetUserById(int id)
        {
            return _context.Authentication
                .FirstOrDefault(u => u.Id == id);
        }

        // This method validates the user's credentials
        public bool ValidateUserCredentials(string username, string password)
        {
            var user = _context.Authentication
                .FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                return false;
            }

            // Simple password check, replace with hashed password validation in production
            return user.Password == password;
        }
    }
}
