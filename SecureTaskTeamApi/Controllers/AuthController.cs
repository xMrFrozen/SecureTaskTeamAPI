using BCrypt.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SecureTaskTeamApi.Data;
using SecureTaskTeamApi.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using static SecureTaskTeamApi.Models.LoginRequest;

namespace SecureTaskTeamApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        public AuthController(ILogger<AuthController> logger, IConfiguration config, AppDBContext dbcontext)
        {
            _config = config;
            _dbcontext = dbcontext;
            _logger = logger;
        }

        private readonly AppDBContext _dbcontext;

        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            if (user.Username.Length < 3) return BadRequest("Username must be at least 3 characters long.");
            if (user.PasswordHash.Length < 6) return BadRequest("Password must be at least 6 characters long.");

            if (await _dbcontext.Users.AnyAsync(u => u.Username == user.Username))
            {
                _logger.LogInformation("Someone attempted to register with an already existing username: " + user.Username);
                return BadRequest("This username is unavailable.");
            }

            string hashedPW = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            user.PasswordHash = hashedPW;

            _dbcontext.Users.Add(user);
            await _dbcontext.SaveChangesAsync();
            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Models.LoginRequest request)
        {
            var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null)
            {
                _logger.LogWarning("Someone attempted to login with an invalid username: " + request.Username);
                return BadRequest("Invalid username or password.");
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                _logger.LogWarning("Someone attempted to login with an invalid password: " + request.Password);
                return BadRequest("Invalid username or password.");
            }

            var token = GenerateJwtToken(user);
            return Ok(new
            {
                Message = "Login successful! Welcome " + user.Username + "!",
                Token = token
            });
        }

        private readonly IConfiguration _config;

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
