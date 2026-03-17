using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Localization;
using WMS.Application.DTOs;
using WMS.Application.Interfaces;
using WMS.Application.Services;
using WMS.Domain.Entities;
using WMS.Presentation.DTOs.Auth;
using WMS.Presentation.Services;
using WMS.Presentation.Services;
using WMS.Presentation.Utilities;

namespace WMS.Presentation.Controllers
{
    [EnableRateLimiting("AuthLimiter")]
    [ApiController]
    [Route("api/Auth")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly IUserService _UserService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public AuthController(JWTSettings jWTSettings, IUserService userService, IMapper mapper,IStringLocalizer<SharedResource> localizer)
        {
            _tokenService = new TokenService(jWTSettings);
            _UserService = userService;
            _mapper = mapper;
            _localizer = localizer;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Username))
                return BadRequest(new ApiResponse<object>
                {
                    Message = _localizer["InvalidRequest"].Value
                });


            var User = await _UserService.GetByUsername(request.Username);


            if (User == null || !BCrypt.Net.BCrypt.Verify(request.Password, User.Password))
                return Unauthorized(new { message = _localizer["InvalidUserLoginRequest"].Value });

            if (!User.IsActive)
                return Unauthorized(new { message = _localizer["InvalidUserLoginAllowed"].Value });

            string accessToken = _tokenService.GenerateAccessToken(
                new UserAccessTokenModel(User.UserID, User.Username, User.Role));

            string refreshToken = _tokenService.GenerateRefreshToken();

            User.RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken);
            User.RefreshTokenExpiredAt = DateTime.UtcNow.AddDays((request.RememberMe ? 7 : 1));
            User.RefreshTokenRevokedAt = null;

            if (await _UserService.Update(User))
            {
                _SetRefreshTokenCookie(refreshToken, User.RefreshTokenExpiredAt.Value);

                var UserDto = _mapper.Map<UserSlimDto>(User);

                return Ok(ApiResponse<object>.SuccessResponse(
                    data: new { User = UserDto, AccessToken = accessToken, FirstName = User.PersonInfo.FirstName },
                    message: _localizer["LoginSuccess"].Value,
                    code: ResultCode.Success
                ));
            }

            return StatusCode(500, _localizer["ServerError"].Value);
        }

        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized(ApiResponse<object>.FailureResponse(
                   message: _localizer["InvalidRefreshToken"].Value,
                   code: ResultCode.Unauthorized
                   ));

            var User = await _UserService.GetByUsername(request.Username);

            if (User == null)
                return BadRequest(ApiResponse<object>.FailureResponse(
                    message: _localizer["InvalidRefreshRequest"].Value,
                    code: ResultCode.InvalidRequest
                    ));

            if (!User.IsActive)
                return Unauthorized(ApiResponse<object>.FailureResponse(
                    message: _localizer["InvalidUserLoginAllowed"].Value,
                    code: ResultCode.Unauthorized
                    ));

            if (User.RefreshTokenRevokedAt != null)
                return Unauthorized(ApiResponse<object>.FailureResponse(
                    message: _localizer["RefreshTokenIsRevoked"].Value,
                    code: ResultCode.Unauthorized
                    ));


            if (User.RefreshTokenExpiredAt == null || User.RefreshTokenExpiredAt <= DateTime.UtcNow)
                return Unauthorized(ApiResponse<object>.FailureResponse(
                    message: _localizer["RefreshTokenExpired"].Value,
                    code: ResultCode.Unauthorized
                    ));

            bool refreshValid = BCrypt.Net.BCrypt.Verify(refreshToken, User.RefreshTokenHash);
            if (!refreshValid)
                return Unauthorized(ApiResponse<object>.FailureResponse(
                    message: _localizer["InvalidRefreshToken"].Value,
                    code: ResultCode.Unauthorized
                    ));

            UserAccessTokenModel userAccessTokenModel =
                new UserAccessTokenModel(User.UserID, User.Username, User.Role);

            string newAccessToken = _tokenService.GenerateAccessToken(userAccessTokenModel);
            string newRefreshToken = _tokenService.GenerateRefreshToken();

            User.RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(newRefreshToken);
            User.RefreshTokenExpiredAt = DateTime.UtcNow.AddDays(7);

            await _UserService.Update(User);

            var tokenResponse = new TokenResponse
            {
                AccessToken = newAccessToken
            };

            _SetRefreshTokenCookie(newRefreshToken, User.RefreshTokenExpiredAt.Value);

            return Ok(ApiResponse<TokenResponse>.SuccessResponse(
                data: tokenResponse,
                message: _localizer["UpdateRefreshToken"].Value,
                code: ResultCode.Success
            ));

        }

        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (request == null || string.IsNullOrWhiteSpace(request.Username))
                return BadRequest(ApiResponse<object>.FailureResponse(
                    message: _localizer["InvalidRefreshRequest"].Value,
                    code: ResultCode.InvalidRequest
                    ));

            var User = await _UserService.GetByUsername(request.Username);

            if (User == null)
                return NotFound(ApiResponse<object>.FailureResponse(
                    message: _localizer["UserNotFound"].Value,
                    code: ResultCode.InvalidRequest
                    ));

            User.RefreshTokenRevokedAt = DateTime.UtcNow;
            await _UserService.Update(User);

            Response.Cookies.Delete("refreshToken");
            return Ok(ApiResponse<object>.SuccessResponse(
                           data: null,
                           message: _localizer["LogoutSuccess"].Value,
                           code: ResultCode.Success
                       ));
        }

        private void _SetRefreshTokenCookie(string token, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, 
                Secure = true,   
                SameSite = SameSiteMode.None,
                Expires = expires,
                Path = "/"
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        [Authorize]
        [HttpGet("validate-session")]
        public IActionResult ValidateSession()
        {
            return Ok (ApiResponse<object>.SuccessResponse(
                message: "Session is active",
                code: ResultCode.ServerWork
                ));
        }

        //[HttpGet("me")]
        //[Authorize]
        //public IActionResult GetCurrentUser()
        //{
        //    var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        //    var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
        //    var permissions = User.FindFirst("Permissions")?.Value;

        //    return Ok(new
        //    {
        //        UserId = userId,
        //        Role = role,
        //        PermissionsBitmask = permissions
        //    });
        //}


    }


}