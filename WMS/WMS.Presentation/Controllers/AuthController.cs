using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Presentation.Controllers
{
    [ApiController]
    [Route("api/Auth")]
    public class AuthController
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        [Route("Login")]
        public async ActionResult<object> Login(User d)
        {
            User user = await _userService.GetByUsername(d.Username);

            user.Username = d.Username;
        }
    }
}
