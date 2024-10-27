using System.Security.Cryptography;
using System.Text;

namespace Auth0Maui.UserServices.Models
{
    public class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(bytes);

                // Convert byte array to a hexadecimal string
                StringBuilder hashString = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    hashString.Append(b.ToString("x2"));
                }
                return hashString.ToString();
            }
        }
    }
}
