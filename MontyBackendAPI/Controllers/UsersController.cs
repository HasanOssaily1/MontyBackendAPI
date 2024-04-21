using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MontyBackendAPI.Models;
using MontyBackendAPI.Repositories;

namespace MontyBackendAPI.Controllers
{

    [Route("api/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUsersRepository usersRepository, ILogger<UsersController> logger)
        {
            _usersRepository = usersRepository;
            _logger = logger;
           
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Users>>> GetAllUsers()
        {
            
            var users = await _usersRepository.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUserById(int id)
        {

            var user = await _usersRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<Users>> CreateUser(Users user)
        {
            user.password = AuthController.HashPassword(user.password);
            user.creationdate = DateTime.UtcNow;
            user.modificationdate = DateTime.UtcNow;
            bool result = await _usersRepository.Create(user);
            if (result)
            {
                return Ok();
            }
          return BadRequest("Email Already exists");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, Users user)
        {
            if (id != user.id)
            {
                return BadRequest();
            }
            
            user.modificationdate = DateTime.UtcNow;
            await _usersRepository.Update(user);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleted = await _usersRepository.Delete(id);
            if (!deleted)
            {
                return BadRequest();
            }
            return Ok();
        }

    }
}
