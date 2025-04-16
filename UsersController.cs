using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SimpleAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private static List<User> Users = new List<User>();

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        // GET api/user
        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            _logger.LogInformation("Fetching all users.");
            return Ok(Users);
        }

        // GET api/user/5
        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {Id} not found.", id);
                return NotFound();
            }
            return Ok(user);
        }

        // POST api/user
        [HttpPost]
        public ActionResult Post([FromBody] User newUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }

            newUser.Id = Users.Count + 1;
            Users.Add(newUser);
            _logger.LogInformation("Added new user with ID {Id}.", newUser.Id);
            return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
        }

        // PUT api/user/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] User updatedUser)
        {
            var existingUser = Users.FirstOrDefault(u => u.Id == id);
            if (existingUser == null)
            {
                _logger.LogWarning("User with ID {Id} not found for update.", id);
                return NotFound();
            }

            existingUser.Name = updatedUser.Name;
            existingUser.Email = updatedUser.Email;
            _logger.LogInformation("Updated user with ID {Id}.", id);
            return NoContent();
        }

        // DELETE api/user/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {Id} not found for deletion.", id);
                return NotFound();
            }

            Users.Remove(user);
            _logger.LogInformation("Deleted user with ID {Id}.", id);
            return NoContent();
        }
    }

    // User model
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
