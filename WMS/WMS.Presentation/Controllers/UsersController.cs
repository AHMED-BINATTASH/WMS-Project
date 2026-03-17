using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WMS.Application.DTOs;
using WMS.Application.Interfaces;
using WMS.Application.Services;
using WMS.Application.Utilities;
using WMS.Domain.Entities;
using WMS.Presentation.Utilities;

namespace WMS.Presentation.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _UserService;
        private readonly IPersonService _PersonService;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IPersonService personService, IStringLocalizer<SharedResource> localizer, IMapper mapper)
        {
            _UserService = userService;
            _PersonService = personService;
            _localizer = localizer;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<UserDto> users = await _UserService.GetAll();

            var Users = _mapper.Map<IEnumerable<UserSlimDto>>(users);
            if (!Users.Any())
            {
                return NotFound(ApiResponse<object>.FailureResponse(message: _localizer["No_Users_Found"]));
            }

            return Ok(ApiResponse<IEnumerable<UserSlimDto>>.SuccessResponse(
                data: Users,
                code: ResultCode.Success,
                message: _localizer["Users_Found"]
            ));
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetById(int id, [FromServices] IAuthorizationService authorizationService)
        {
            if (id < 1)
                return BadRequest(ApiResponse<object>.FailureResponse(
                    message: _localizer["Invalid_ID"],
                    code: ResultCode.InvalidRequest));

            var UserDto = await _UserService.GetByID(id);

            if (UserDto == null)
            {
                return NotFound(ApiResponse<UserSlimDto>.FailureResponse(
                    message: _localizer["User_Not_Found"],
                    code: ResultCode.NotFound));
            }

            var User = _mapper.Map<UserSlimDto>(UserDto);
            var authResult = await authorizationService.AuthorizeAsync(base.User, User.UserID, "UserOwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid();

            return Ok(ApiResponse<UserSlimDto>.SuccessResponse(
                data: User,
                message: _localizer["User_Found"],
                code: ResultCode.Success
            ));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Add([FromBody] UserAddDto UserToAddDto)
        {
            if (string.IsNullOrWhiteSpace(UserToAddDto.Username) || string.IsNullOrWhiteSpace(UserToAddDto.Password))
                return BadRequest(ApiResponse<object>.FailureResponse(message: _localizer["Username_Password_Required"]));

            bool isPersonExist = await _PersonService.IsExistByPersonID(UserToAddDto.PersonID);
            if (!isPersonExist)
                return BadRequest(ApiResponse<object>.FailureResponse(message: _localizer["Person_Not_Found"]));

            bool isUsernameTaken = await _UserService.IsUsernameExist(UserToAddDto.Username);
            if (isUsernameTaken)
                return BadRequest(ApiResponse<object>.FailureResponse(message: _localizer["Username_Already_Taken"]));

            bool isAlreadyUsername = await _UserService.IsPersonExist(UserToAddDto.PersonID);
            if (isAlreadyUsername)
                return BadRequest(ApiResponse<object>.FailureResponse(message: _localizer["Person_Already_User"])); 
            
            User userEntity = _mapper.Map<User>(UserToAddDto);

            userEntity.Username = userEntity.Username.Trim();
            userEntity.Password = BCrypt.Net.BCrypt.HashPassword(UserToAddDto.Password);

            var isAdded = await _UserService.AddNew(userEntity);

            if (isAdded)
            {
                var createdUser = _mapper.Map<UserSlimDto>(userEntity);
                return CreatedAtAction(
                    nameof(GetById),
                    new { id = userEntity.UserID },
                    ApiResponse<UserSlimDto>.SuccessResponse(
                        data: createdUser,
                        message: _localizer["User_Created"],
                        code: ResultCode.Success)
                );
            }

            return StatusCode(500, ApiResponse<object>.FailureResponse(message: _localizer["Server_Error"]));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] UserSlimDto requestUserCrudDto)
        {
            if (requestUserCrudDto == null || requestUserCrudDto.UserID <= 0)
                return BadRequest(ApiResponse<object>.FailureResponse(message: _localizer["Invalid_Request"]));

            // 1. التأكد من وجود المستخدم الأصلي في الداتابيز
            var currentUser = await _UserService.GetByID(requestUserCrudDto.UserID);
            if (currentUser == null)
                return NotFound(ApiResponse<object>.FailureResponse(message: _localizer["User_Not_Found"]));

            // 2. إذا تم تغيير الـ PersonID، نتحقق من وجوده وأنه غير مستخدم من حساب آخر
            if (requestUserCrudDto.PersonID != currentUser.PersonID)
            {
                bool isPersonExist = await _PersonService.IsExistByPersonID(requestUserCrudDto.PersonID);
                if (!isPersonExist)
                    return BadRequest(ApiResponse<object>.FailureResponse(message: _localizer["Person_Not_Found"]));

                bool isAlreadyUser = await _UserService.IsPersonExist(requestUserCrudDto.PersonID);
                if (isAlreadyUser)
                    return BadRequest(ApiResponse<object>.FailureResponse(message: _localizer["Person_Already_User"]));
            }

            // 3. التحقق من اسم المستخدم: إذا تغير، نتأكد أنه غير محجوز لغيره
            if (requestUserCrudDto.Username.Trim().ToLower() != currentUser.Username.Trim().ToLower())
            {
                bool isUsernameTaken = await _UserService.IsUsernameExist(requestUserCrudDto.Username.Trim());
                if (isUsernameTaken)
                    return BadRequest(ApiResponse<object>.FailureResponse(message: _localizer["Username_Already_Taken"]));
            }

            // 4. عملية الـ Mapping (يفضل استخدام الـ Entity الموجود أصلاً وتحديثه لتجنب مشاكل الـ Tracking)
            var User = _mapper.Map<User>(currentUser);

            var PersonDto =  await _PersonService.GetByID(User.PersonID);

            User.PersonInfo = _mapper.Map<Person>(PersonDto);

            var result = await _UserService.Update(User);

            if (result)
            {
                return Ok(ApiResponse<object>.SuccessResponse(
                    message: _localizer["User_Updated"],
                    code: ResultCode.Success));
            }

            return StatusCode(500, ApiResponse<object>.FailureResponse(message: _localizer["Server_Error"]));
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _UserService.Delete(id);
            if (result)
            {
                return Ok(ApiResponse<object>.SuccessResponse(message: _localizer["User_Deleted"]));
            }

            return NotFound(ApiResponse<object>.FailureResponse(message: _localizer["User_Not_Found"]));
        }
    }
}