using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WMS.Application.DTOs;
using WMS.Application.Interfaces;
using WMS.Application.Services;
using WMS.Domain.Entities;
using WMS.Presentation.Utilities;

namespace WMS.Presentation.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly JWTSettings _jWTSettings;
        private readonly IService<UserDto, User> _UserService;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IMapper _mapper;

        public UserController(JWTSettings jWTSettings, IService<UserDto,User> UserService, IStringLocalizer<SharedResource> localizer, IMapper mapper)
        {
            _jWTSettings = jWTSettings;
            _UserService = UserService;
            _localizer = localizer;
            _mapper = mapper;
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<UserDto> Users = await _UserService.GetAll();

            if (Users.Count() == 0)
            {
                return NotFound(ApiResponse<object>.FailureResponse(message: _localizer["NoUsersFound"]));
            }

            return Ok(ApiResponse<IEnumerable<UserDto>>.SuccessResponse(
                data: Users
                ));
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var User = await _UserService.GetByID(id);

            if (User == null)
            {
                return NotFound(ApiResponse<object>.FailureResponse(
                    message: _localizer["UserNotFound"],
                    code: ResultCode.NotFound));
            }

            return Ok(ApiResponse<UserDto>.SuccessResponse(data: User));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] UserDto UserDto)
        {
            if (UserDto == null)
            {
                return BadRequest(ApiResponse<object>.FailureResponse(
                    message: _localizer["InvalidRequest"],
                    code: ResultCode.InvalidRequest));
            }

            User User = _mapper.Map<User>(UserDto);
            // Assuming your service returns the created object or its ID
            var IsAdded = await _UserService.AddNew(User);

            if (IsAdded)
            {
                return CreatedAtAction(
                    nameof(GetById),
                    new { id = UserDto.UserID },
                    ApiResponse<object>.SuccessResponse(
                        message: _localizer["UserCreated"],
                        code: ResultCode.Success)
                );
            }

            return StatusCode(500, ApiResponse<object>.FailureResponse(
                message: _localizer["ServerError"]));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] UserDto UserDto)
        {
            if (UserDto == null)
            {
                return BadRequest(ApiResponse<object>.FailureResponse(message: _localizer["InvalidRequest"]));
            }

            User User = _mapper.Map<User>(UserDto);

            var result = await _UserService.Update(User);
            if (result)
            {
                return Ok(ApiResponse<object>.SuccessResponse(
                    message: _localizer["UserUpdated"],
                    code: ResultCode.Success));
            }

            return NotFound(ApiResponse<object>.FailureResponse(message: _localizer["UserNotFound"]));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _UserService.Delete(id);
            if (result)
            {
                return Ok(ApiResponse<object>.SuccessResponse(
                    message: _localizer["UserDeleted"],
                    code: ResultCode.Success));
            }

            return NotFound(ApiResponse<object>.FailureResponse(
                message: _localizer["UserNotFound"],
                code: ResultCode.NotFound));
        }
    }
}
