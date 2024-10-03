using Auth0Maui.UserServices.Models;
namespace Auth0Maui.UserServices.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/authentification")]
    public class AuthenticationController : ControllerBase
    {
        private static List<AuthenticationItem> Users = new List<AuthenticationItem>();

        // GET: api/authentification
        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(Users);
        }

        // GET: api/authentification/{id}
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // POST: api/authentification
        [HttpPost]
        public IActionResult AddUser([FromBody] AuthenticationItem newUser)
        {
            Users.Add(newUser);
            return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
        }

        // PUT: api/authentification/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] AuthenticationItem updatedUser)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();
            user.UserName = updatedUser.UserName;
            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Password = updatedUser.Password;
            return NoContent();
        }

        // DELETE: api/authentification/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteUserById(int id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();
            Users.Remove(user);
            return NoContent();
        }
    }

   

}
