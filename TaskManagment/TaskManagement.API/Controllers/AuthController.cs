using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // 🔐 Hardcoded users (NO DB)
        private static readonly Dictionary<string, (string Password, string Role, string UserId)> Users =
            new()
            {
                { "admins", ("admin1234", "Admin", "admin-001") },
                { "users", ("user1234", "User", "user-001") },
                { "suchitra655@gmail.com", ("Suchitra1234", "User", "user-002") },
                { "lakshmi475@gmail.com", ("Lakshmi1234", "User", "user-003") },
                { "abhilashSb@gmail.com", ("Abjilash1234", "User", "user-004") },
            };

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (!Users.TryGetValue(request.Username, out var user) ||
                user.Password != request.Password)
            {
                return Unauthorized("Invalid credentials");
            }

            var token = GenerateJwtToken(request.Username, user.Role, user.UserId);

            return Ok(new
            {
                token,
                role = user.Role,
                userId = user.UserId
            });
        }

        private string GenerateJwtToken(string username, string role, string userId)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim("userId", userId),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.NameIdentifier, userId)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("THIS_IS_A_SUPER_SECRET_KEY_12345"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "TaskManagement.API",
                audience: "TaskManagement.API",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
    public class LoginRequest
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
