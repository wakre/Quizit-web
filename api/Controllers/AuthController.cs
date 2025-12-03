using api.DTOs;
using api.Models;
using api.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _logger;

        public AuthController(AppDbContext db, IConfiguration config, ILogger<AuthController> logger)
        {
            _db = db;
            _config = config;
            _logger = logger;
        }

        // FOR REGISTER A USER (Manual mapping DTO -> Entity)
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check duplicate emails
            var existingUser = _db.Users.FirstOrDefault(u => u.Email == dto.Email);
            if (existingUser != null)
                return BadRequest(new { message = "This email is already in use !!" });

            // MANUAL MAPPING: UserRegisterDto -> User Entity
            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password) // Hash password
            };

            try
            {
                _db.Users.Add(user);
                await _db.SaveChangesAsync();

                _logger.LogInformation("User registered: {email}", dto.Email);

                // MANUAL MAPPING: User Entity -> UserDto (returns basic profile info)
                var userDto = new UserDto
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    Email = user.Email
                };

                return Ok(new
                {
                    message = "User registered successfully!",
                    user = userDto
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering user");
                return StatusCode(500, "Error while creating user.");
            }
        }

        // LOGIN AND RETURN TOKEN + USER INFO
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _db.Users.FirstOrDefault(u => u.Email == dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized(new { message = "Invalid email or password." });

            // Generate JWT
            string token = GenerateJwtToken(user);

            _logger.LogInformation("User logged in: {email}", dto.Email);

            // MANUAL MAPPING: User -> UserDto
            var userDto = new UserDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email
            };

            return Ok(new
            {
                token,
                user = userDto
            });
        }

        // GENERATE JWT TOKEN
        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Claims manually mapped from User entity
            var claims = new[]
            {
                new Claim("userId", user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(4),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
