using Microsoft.AspNetCore.Mvc;

namespace ApiSecurity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        public record AuthenticateData(string? UserName, string? Password);

        public record UserData(int UserId, string Username);

        [HttpPost("token")]
        public ActionResult<string> Authenticate([FromBody] AuthenticateData data)
        {


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
