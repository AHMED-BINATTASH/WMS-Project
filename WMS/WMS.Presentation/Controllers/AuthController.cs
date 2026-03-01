using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace WMS.Presentation.Controllers
{
    [ApiController]
    [Route("api/Auth")]
    public class AuthController
    {
        [HttpPost]
        [Route("Login")]
        public ActionResult<object> Login([FromBody]string username,[FromBody]string password)
        {
            var obj = new { username, password };
            return obj;
        }
    }
}
