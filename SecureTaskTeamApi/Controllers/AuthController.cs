using BCrypt.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureTaskTeamApi.Data;
using SecureTaskTeamApi.Models;
using static SecureTaskTeamApi.Models.LoginRequest;

namespace SecureTaskTeamApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDBContext _dbcontext;
        public AuthController(AppDBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            if (await _dbcontext.Users.AnyAsync(u => u.Username == user.Username)) return BadRequest("This username is unavailable.");

            string hashedPW = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

            user.PasswordHash=hashedPW;

            _dbcontext.Users.Add(user);
            await _dbcontext.SaveChangesAsync();

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Models.LoginRequest request)
        {
            var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null) return BadRequest("Invalid username or password.");

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            if(!isPasswordValid) return BadRequest("Invalid username or password.");

            return Ok("Login successful! Welcome " + user.Username + "!");
        }
    }
}
