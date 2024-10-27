using Auth0Maui.UserServices.Data;
using Auth0Maui.UserServices.Models;
using Auth0Maui.UserServices.Models.Auth0Maui.UserServices.Models;
using Auth0Maui.UserServices.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth0Maui.UserServices.Controllers
{
    [ApiController]
    [Route("api/authentification")]
    [Authorize]
    public class AuthenticationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthenticationController(ApplicationDbContext context, IUserService userService, IConfiguration configuration)
        {
            _context = context;
            _userService = userService;
            _configuration = configuration;
        }

        /// <summary>
        /// Logs in a user and generates a JWT token.
        /// </summary>
        /// <param name="loginModel">Login credentials (username and password).</param>
        /// <returns>JWT token if login is successful.</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Logs in a user and generates a JWT token.")]
        [SwaggerResponse(200, "Login successful", typeof(string))]
        [SwaggerResponse(401, "Invalid username or password")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            var user = _context.Authentication.FirstOrDefault(u => u.UserName == loginModel.Username);

            if (user == null || !VerifyPassword(loginModel.Password, user.Password))
            {
                return Unauthorized("Invalid username or password.");
            }

            var token = GenerateJwtToken(user.UserName);
            return Ok(new { Token = token });
        }

        /// <summary>
        /// Adds a new user with a hashed password.
        /// </summary>
        /// <param name="newUser">The user to be added (ID will be auto-generated).</param>
        /// <returns>The newly created user.</returns>
        [HttpPost("addUser")]
        [Authorize]
        [SwaggerOperation(Summary = "Adds a new user (ID is auto-generated).")]
        [SwaggerResponse(201, "User created", typeof(AuthenticationItem))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<IActionResult> AddUser([FromBody] CreateUserDto newUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Hash the password before storing it
            var authenticationItem = new AuthenticationItem
            {
                UserName = newUser.UserName,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Password = PasswordHasher.HashPassword(newUser.Password) // Hash the password
            };

            _context.Authentication.Add(authenticationItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserById), new { id = authenticationItem.Id }, authenticationItem);
        }

        // PUT: api/authentification/updateUser/{id}
        [HttpPut("updateUser/{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Updates an existing user.")]
        [SwaggerResponse(204, "User updated successfully")]
        [SwaggerResponse(404, "User not found")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] AuthenticationItem updatedUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Authentication.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.UserName = updatedUser.UserName;
            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Password = PasswordHasher.HashPassword(updatedUser.Password); // Hash the password

            _context.Authentication.Update(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/authentification/deleteUser/{id}
        [HttpDelete("deleteUser/{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Deletes a user by ID.")]
        [SwaggerResponse(204, "User deleted successfully")]
        [SwaggerResponse(404, "User not found")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Authentication.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Authentication.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/authentification/getUser/{id}
        [HttpGet("getUser/{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Gets a user by ID.")]
        [SwaggerResponse(200, "User found", typeof(AuthenticationItem))]
        [SwaggerResponse(404, "User not found")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _context.Authentication.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        private string GenerateJwtToken(string username)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Hash the password entered during login and compare it with the stored hashed password
        private bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            var inputHashed = PasswordHasher.HashPassword(inputPassword); // Hash the input password
            return inputHashed == hashedPassword; // Compare the hashed passwords
        }
    }
}
