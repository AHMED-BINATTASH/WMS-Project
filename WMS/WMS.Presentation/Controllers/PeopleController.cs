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
    [Route("api/People")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly JWTSettings _jWTSettings;
        private readonly IPersonService _PersonService;
        private readonly ICountryService _CountryService;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IMapper _mapper;

        public PersonController(JWTSettings jWTSettings, IPersonService PersonService, ICountryService countryService, IStringLocalizer<SharedResource> localizer, IMapper mapper)
        {
            _jWTSettings = jWTSettings;
            _PersonService = PersonService;
            _CountryService = countryService;
            _localizer = localizer;
            _mapper = mapper;
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<PersonDto> People = await _PersonService.GetAll();

            if (People.Count() == 0)
            {
                return NotFound(ApiResponse<object>.FailureResponse(message: _localizer["No_People_Found"]));
            }

            return Ok(ApiResponse<IEnumerable<PersonDto>>.SuccessResponse(
                data: People,
                code: ResultCode.Success,
                message: _localizer["People_Found"]
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

            var person = await _PersonService.GetByID(id);

            if (person == null)
            {
                return NotFound(ApiResponse<object>.FailureResponse(
                    message: _localizer["PersonNotFound"],
                    code: ResultCode.NotFound));
            }

            var authResult = await authorizationService.AuthorizeAsync(User, person.PersonID, "PersonOwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid();

            return Ok(ApiResponse<PersonDto>.SuccessResponse(
                data: person,
                message: _localizer["Person_Found"],
                code: ResultCode.Success
            ));
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] PersonDto PersonDto)
        {
            Person Person = _mapper.Map<Person>(PersonDto); 
            string Msg = _CleanAndValidatePerson(ref Person);

            if (!string.IsNullOrEmpty(Msg))
            {
                return BadRequest(ApiResponse<object>.FailureResponse(
                    message: Msg,
                    code: ResultCode.InvalidRequest));
            }

            bool IsExist = await _PersonService.IsExistByNationalID(PersonDto.NationalID.ToCleanPascalCase()) ||
                await _PersonService.IsExistByEmail(PersonDto.Email.ToCleanPascalCase());

            if (IsExist)
                return BadRequest(ApiResponse<object>.FailureResponse(
                   message: _localizer["Person_Already_Exist"],
                   code: ResultCode.AlreadyExists
                   ));

            bool IsCountryExist = await _CountryService.IsExistByCountryID(PersonDto.CountryID);

            if(!IsCountryExist)
                return BadRequest(ApiResponse<object>.FailureResponse(
                   message: _localizer["CountryID_Not_Found"],
                   code: ResultCode.InvalidRequest
                   ));

            var IsAdded = await _PersonService.AddNew(Person);

            if (IsAdded)
            {
                PersonDto createdPersonDto = _mapper.Map<PersonDto>(Person);
                return CreatedAtAction(
                    nameof(GetById),
                    new { id = Person.PersonID },
                    ApiResponse<PersonDto>.SuccessResponse(
                        data: createdPersonDto,
                        message: _localizer["Person_Created"],
                        code: ResultCode.Success)
                );
            }

            return StatusCode(500, ApiResponse<object>.FailureResponse(
                message: _localizer["Server_Error"]));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] PersonDto RequestPersonDto)
        {
            if (RequestPersonDto == null || RequestPersonDto.PersonID <= 0)
            {
                return BadRequest(ApiResponse<object>.FailureResponse(
                    message: _localizer["Invalid_Request"],
                    code: ResultCode.InvalidRequest));
            }

            var currentPersonDto = await _PersonService.GetByID(RequestPersonDto.PersonID);
            if (currentPersonDto == null)
            {
                return NotFound(ApiResponse<object>.FailureResponse(
                    message: _localizer["Person_Not_Found"],
                    code: ResultCode.NotFound));
            }

            Person personToUpdate = _mapper.Map<Person>(RequestPersonDto);
            string validationMsg = _CleanAndValidatePerson(ref personToUpdate);

            if (!string.IsNullOrEmpty(validationMsg))
            {
                return BadRequest(ApiResponse<object>.FailureResponse(
                    message: validationMsg,
                    code: ResultCode.InvalidRequest));
            }

            if (string.Equals(currentPersonDto.NationalID, personToUpdate.NationalID) &&
                string.Equals(currentPersonDto.Email, personToUpdate.Email) &&
                string.Equals(currentPersonDto.FirstName, personToUpdate.FirstName) &&
                string.Equals(currentPersonDto.LastName, personToUpdate.LastName) &&
                string.Equals(currentPersonDto.Phone, personToUpdate.Phone) &&
                string.Equals(currentPersonDto.Address, personToUpdate.Address) &&
                currentPersonDto.CountryID == personToUpdate.CountryID)
            {
                return Ok(ApiResponse<object>.SuccessResponse(
                    message: _localizer["Person_Updated"],
                    code: ResultCode.Success));
            }

            bool IsExist = (await _PersonService.IsExistByNationalID(personToUpdate.NationalID) && personToUpdate.NationalID != currentPersonDto.NationalID) ||
                           (await _PersonService.IsExistByEmail(personToUpdate.Email) && personToUpdate.Email != currentPersonDto.Email);

            if (IsExist)
            {
                return BadRequest(ApiResponse<object>.FailureResponse(
                    message: _localizer["Person_Already_Exist"],
                    code: ResultCode.AlreadyExists));
            }

            var result = await _PersonService.Update(personToUpdate);

            if (result)
            {
                return Ok(ApiResponse<object>.SuccessResponse(
                    message: _localizer["Person_Updated"],
                    code: ResultCode.Success));
            }

            return StatusCode(500, ApiResponse<object>.FailureResponse(
                message: _localizer["Server_Error"],
                code: ResultCode.InternalError));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _PersonService.Delete(id);
            if (result)
            {
                return Ok(ApiResponse<object>.SuccessResponse(
                    message: _localizer["Person_Deleted"],
                    code: ResultCode.Success));
            }

            return NotFound(ApiResponse<object>.FailureResponse(
                message: _localizer["Person_Not_Found"],
                code: ResultCode.NotFound));
        }
        private string _CleanAndValidatePerson(ref Person person)
        {
            if (person == null) return _localizer["InvalidData"];

            var properties = typeof(Person).GetProperties();

            foreach (var prop in properties)
            {
                if (prop.Name == "PersonID") continue;

                var value = prop.GetValue(person);

                if (prop.PropertyType == typeof(string))
                {
                    var stringValue = value?.ToString();

                    if (!string.IsNullOrWhiteSpace(stringValue))
                    {
                        stringValue = stringValue.ToCleanPascalCase();
                        prop.SetValue(person, stringValue);
                    }

                    // 2. التحقق (Validation)
                    if (string.IsNullOrWhiteSpace(stringValue))
                        return _localizer[$"{prop.Name}Required"];

                    if (prop.Name == "NationalID" && !stringValue.All(char.IsDigit))
                        return _localizer["NationalIDOnlyNumbers"];

                    if ((prop.Name == "FirstName" || prop.Name == "LastName") && !stringValue.All(char.IsLetter))
                        return _localizer[$"{prop.Name}OnlyLetters"];

                    if (prop.Name == "Phone")
                    {
                        if (!stringValue.All(char.IsDigit)) return _localizer["PhoneOnlyNumbers"];
                        if (stringValue.Length > 15) return _localizer["PhoneMaxLength"];
                    }

                    if (prop.Name == "Email" && !_IsValidEmail(stringValue))
                        return _localizer["InvalidEmailFormat"];
                }
                else if (prop.PropertyType == typeof(int) && (int)(value ?? 0) <= 0)
                {
                    return _localizer[$"{prop.Name}Invalid"];
                }
            }

            return null;
        }
        private bool _IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
