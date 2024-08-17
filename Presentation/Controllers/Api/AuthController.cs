using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Presentation.Models;

namespace Presentation.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public AuthController(IConfiguration configuration, IUserRepository userRepository)
        {
            Configuration = configuration;
            UserRepository = userRepository;
        }

        private IUserRepository UserRepository { get; }

        private IConfiguration Configuration { get; }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login login, CancellationToken cancellation)
        {
            var user = await UserRepository.GetUser(login.Username, cancellation);

            if (user == null || user.Password != login.Password)
            {
                return BadRequest();
            }

            var token = GenerateJwtToken(login.Username);
            return Ok(new { token });
        }

        private string GenerateJwtToken(string username)
        {
            var jwtSettings = Configuration.GetSection("Jwt");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT_SECRET_KEY"]!));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(jwtSettings["ExpireMinutes"]!)),
                signingCredentials: cred);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
