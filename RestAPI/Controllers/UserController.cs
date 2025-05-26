using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestAPI.ViewModel;
using RestAPI.Services;
using RestAPI.Data;
using RestAPI.Helpers;
using Microsoft.AspNetCore.Identity;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly UserService _userService;

        public UserController(ApplicationDbContext dbcontext, UserService userService)
        {
            _dbcontext = dbcontext;
            _userService = userService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            if (user == null || string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Invalid user data.");
            }

            if (!PasswordHelper.IsPasswordValid(user.Password, out string errorMessage))
            {
                return BadRequest(errorMessage);
            }

            var existingUser = UserService.GetUser(user.UserName);
            if (existingUser != null)
            {
                return Conflict("User already exists.");
            }

            user.Password = PasswordHelper.HashPassword(user.Password);

            UserService.Users.Add(user);
            return Ok("Registration successful!");

        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            if (user == null || string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Invalid user data. Username and password are required.");
            }

            var existingUser = UserService.Users.FirstOrDefault(u => u.UserName == user.UserName && u.Password == user.Password);
            if (existingUser == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            if (!PasswordHelper.VerifyPassword(user.Password, existingUser.Password))
            {
                return BadRequest("Invalid username or password.");
            }

            return Ok("Login successful.");

        }

        [HttpGet("getuser/{userName}")]
        public IActionResult GetUser(string userName)
        {
            var user = UserService.Users.FirstOrDefault(u => u.UserName == userName);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            return Ok(user);
        }
    }
}
