using Microsoft.AspNetCore.Mvc;

namespace ApiSecurity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        public record AuthenticateData(string? UserName, string? Password);

        //[HttpPost("token")]
        //public ActionResult<string> Authenticate([FromBody] string data)
        //{

        //}
    }
}
