using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestAPI.ViewModel;
using RestAPI.Services;
using RestAPI.Data;
using RestAPI.Helpers;
using Microsoft.AspNetCore.Identity;
using RestAPI.Services.Interfaces;
using System.Threading.Tasks;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly IUserService _userService;

        public UserController(ApplicationDbContext dbcontext, IUserService userService)
        {
            _dbcontext = dbcontext;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (user == null || string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Invalid user data.");
            }

            if (!PasswordHelper.IsPasswordValid(user.Password, out string errorMessage))
            {
                return BadRequest(errorMessage);
            }

            var existingUser = await _userService.GetUserAsync(user.UserName);
            if (existingUser != null)
            {
                return Conflict("User already exists.");
            }

            user.Password = PasswordHelper.HashPassword(user.Password);

            await _userService.AddUserAsync(user);
            return Ok("Registration successful!");

        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            if (user == null || string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Invalid user data. Username and password are required.");
            }

            var existingUser = await _userService.GetUserAsync(user.UserName);
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
    }
}
