using LMS_API.Models;
using LMS_API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IConfiguration configuration;
        public TokenController(IUserRepository userRepository, IConfiguration configuration)
        {
            this.userRepository = userRepository;
            this.configuration = configuration;
        }
        [HttpPost]
        public IActionResult Token(AuthenticateUser authenticateUser)
        {
            var checkUser = userRepository.AuthenticateUser(authenticateUser);
            if (checkUser != null)
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name,checkUser.Item1.UserName),
                    new Claim("UserID",Convert.ToString(checkUser.Item1.UserID)) };

                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);
                return Ok(new
                {
                    User = checkUser.Item1,
                    MenuDetails = checkUser.Item2,
                    access_token = new JwtSecurityTokenHandler().WriteToken(token),
                    token_type = "bearer",
                    expires_in = DateTime.Now.AddDays(1),
                });
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
