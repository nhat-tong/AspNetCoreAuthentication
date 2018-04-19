using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace SecureAspnetCoreApi.JWTAuthentication.Controllers
{
    [Produces("application/json")]
    [Route("api/Token")]
    public class TokenController : Controller
    {
        private readonly IConfiguration _config;

        public TokenController(IConfiguration config)
        {
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
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