using Auth0Maui.UserServices.Data;
using Auth0Maui.UserServices.Models;
using Auth0Maui.UserServices.Models.Auth0Maui.UserServices.Models;
using Auth0Maui.UserServices.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth0Maui.UserServices.Controllers
{
    [ApiController]
    [Route("api/authentification")]
    [Authorize] // Apply authorization at the controller level
    public class AuthenticationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        // Inject the ApplicationDbContext, IUserService, and IConfiguration into the controller
        public AuthenticationController(ApplicationDbContext context, IUserService userService, IConfiguration configuration)
        {
            _context = context;
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        [AllowAnonymous] // Allow anonymous access to the login endpoint
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            if (_userService.ValidateUserCredentials(loginModel.Username, loginModel.Password))
            {
                var token = GenerateJwtToken(loginModel.Username);
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid username or password.");
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

        // Other controller actions...

        // GET: api/authentification/{id}
        [HttpGet("{id}")]
        [Authorize] // Secures this specific action
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _context.Authentication.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST: api/authentification
        [HttpPost]
        [Authorize] // Secures this specific actio
        public async Task<IActionResult> AddUser([FromBody] AuthenticationItem newUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Authentication.Add(newUser);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
        }

        // PUT: api/authentification/{id}
        [HttpPut("{id}")]
        [Authorize] // Secures this specific actio
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
            user.Password = updatedUser.Password;

            _context.Authentication.Update(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/authentification/{id}
        [HttpDelete("{id}")]
        [Authorize] // Secures this specific actio
        public async Task<IActionResult> DeleteUserById(int id)
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
    }
}
