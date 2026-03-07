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
        public ActionResult<object> Login(object d)
        {
            var obj = d;
            return obj;
        }
    }
}
