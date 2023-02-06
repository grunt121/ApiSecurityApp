using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace ApiSecurity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public record AuthenticateData(string? UserName, string? Password);

        public record UserData(int UserId, string Username);

        [HttpPost("token")]
        public ActionResult<string> Authenticate([FromBody] AuthenticateData data)
        {

            var user = ValidateCredentials(data);

            if (user == null)
                return Unauthorized();

            var token = GenerateToken(user);
            return Ok(token);


        }

        private string GenerateToken(UserData user)
        {
            var secretKey =
                new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(_configuration.GetValue<string>("Authentication:SecretKey")));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new()
            {
                new(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new(JwtRegisteredClaimNames.UniqueName, user.Username)
            };

            var token = new JwtSecurityToken(
                _configuration.GetValue<string>("Authentication:Issuer"),
                _configuration.GetValue<string>("Authentication:Audience"),
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(1),
                signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        private static UserData? ValidateCredentials(AuthenticateData data)
        {
            //Simple Demo
            if (CompareValues(data.UserName, "jamie") && CompareValues(data.Password,"1234"))
                return new UserData(1,data.UserName!);

            if (CompareValues(data.UserName, "peterparker") && CompareValues(data.Password, "1234"))
                return new UserData(2, data.UserName!);

            return null;

        }

        private static bool CompareValues(string? actual, string expected)
        {
            if (actual is null)
                return false;

            if (actual.Equals(expected,StringComparison.InvariantCultureIgnoreCase))
                return true;

            return false;
        }
    }
}
