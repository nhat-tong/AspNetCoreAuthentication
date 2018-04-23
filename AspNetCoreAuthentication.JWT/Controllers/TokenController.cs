using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AspNetCoreAuthentication.JWT.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Produces("application/json")]
    [Route("api/[Controller]")]
    public class TokenController : Controller
    {
        private readonly IConfiguration _config;

        public TokenController(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Create a Json Web Token
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>  
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult CreateToken([FromBody]LoginModel model)
        {
            // 1. First: try authenticate user with model submitted
            var user = AuthenticateUser(model);
            if (user == null) return Unauthorized();

            // 2. User is authenticated, build and return a JWT token
            var token = BuildToken(user);
            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), expiration = token.ValidTo });
        }

        private JwtSecurityToken BuildToken(UserModel user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.DateOfBirth, user.Birthdate.ToString("yyyy-MM-dd"))
            };

            return new JwtSecurityToken
            (
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(5),
                claims: claims,
                signingCredentials: creds
            );
        }

        private UserModel AuthenticateUser(LoginModel model)
        {
            UserModel user = null;

            if (model.Username == "nhat" && model.Password == "mysecret")
            {
                user = new UserModel { Name = "Nhat TONG", Email = "nhat.tong@domain.com", Birthdate = new DateTime(1990, 8, 3) };
            }
            return user;
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime Birthdate { get; set; }
    }
}