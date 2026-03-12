using AutoMapper;
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
    [Route("api/Countries")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly JWTSettings _jWTSettings;
        private readonly ICountryService _CountryService;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IMapper _mapper;

        public CountryController(JWTSettings jWTSettings, ICountryService CountryService, IStringLocalizer<SharedResource> localizer, IMapper mapper)
        {
            _jWTSettings = jWTSettings;
            _CountryService = CountryService;
            _localizer = localizer;
            _mapper = mapper;
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<CountryDto> Countries = await _CountryService.GetAll();

            if (Countries.Count() == 0)
            {
                return NotFound(ApiResponse<object>.FailureResponse(message: _localizer["No_Countries_Found"]));
            }

            return Ok(ApiResponse<IEnumerable<CountryDto>>.SuccessResponse(
                data: Countries,
                code: ResultCode.Success,
                message: _localizer["Countries_Found"]
                ));
        }
        
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var Country = await _CountryService.GetByID(id);

            if (Country == null)
            {
                return NotFound(ApiResponse<object>.FailureResponse(
                    message: _localizer["CountryNotFound"],
                    code: ResultCode.NotFound));
            }

            return Ok(ApiResponse<CountryDto>.SuccessResponse(
                data: Country,
                message: _localizer["Country_Found"],
                code: ResultCode.Success
                ));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] CountryDto CountryDto)
        {
            if (CountryDto == null || string.IsNullOrWhiteSpace(CountryDto.CountryName) || CountryDto.CountryName.IsAsciiOnly() )
            {
                return BadRequest(ApiResponse<object>.FailureResponse(
                    message: _localizer["Invalid_Request"],
                    code: ResultCode.InvalidRequest));
            }

            bool IsExist = await _CountryService.IsExistByCountryName(CountryDto.CountryName.ToCleanPascalCase());

            if (IsExist)
                return BadRequest(ApiResponse<object>.FailureResponse(
                   message: _localizer["Country_Already_Exist"],
                   code: ResultCode.AlreadyExists
                   ));

            Country country = _mapper.Map<Country>(CountryDto);

            country.CountryName = CountryDto.CountryName.ToCleanPascalCase();

            var IsAdded = await _CountryService.AddNew(country);

            if (IsAdded)
            {
                CountryDto createdCountryDto = _mapper.Map<CountryDto>(country);
                return CreatedAtAction(
                    nameof(GetById),
                    new { id = country.CountryID },
                    ApiResponse<CountryDto>.SuccessResponse(
                        data: createdCountryDto,
                        message: _localizer["Country_Created"],
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
        public async Task<IActionResult> Update([FromBody] CountryDto RequestCountryDto)
        {
            if (RequestCountryDto == null || RequestCountryDto.CountryID <= 0 || string.IsNullOrWhiteSpace(RequestCountryDto.CountryName) || RequestCountryDto.CountryName.IsAsciiOnly())
            {
                return BadRequest(ApiResponse<object>.FailureResponse(
                    message: _localizer["Invalid_Request"],
                    code: ResultCode.InvalidRequest));
            }

            CountryDto countryDto = await _CountryService.GetByID(RequestCountryDto.CountryID);

            if (countryDto == null)
            {
                return NotFound(ApiResponse<object>.FailureResponse(
                    message: _localizer["Country_Not_Found"],
                    code: ResultCode.NotFound));
            }

            if (string.Equals(countryDto.CountryName?.Trim(), RequestCountryDto.CountryName?.Trim().ToCleanPascalCase(), StringComparison.OrdinalIgnoreCase))
            {
                return Ok(ApiResponse<object>.SuccessResponse(
                    message: _localizer["Country_Updated"],
                    code: ResultCode.Success));
            }

            bool IsExist = await _CountryService.IsExistByCountryName(RequestCountryDto.CountryName.ToCleanPascalCase());

            if (IsExist)
            {
                return BadRequest(ApiResponse<object>.FailureResponse(
                   message: _localizer["Country_Already_Exist"],
                   code: ResultCode.AlreadyExists));
            }

            Country country = _mapper.Map<Country>(RequestCountryDto);

            country.CountryName = RequestCountryDto.CountryName.ToCleanPascalCase();

            var result = await _CountryService.Update(country);

            if (result)
            {
                return Ok(ApiResponse<object>.SuccessResponse(
                    message: _localizer["Country_Updated"],
                    code: ResultCode.Success));
            }

            return StatusCode(500, ApiResponse<object>.FailureResponse(
                message: _localizer["Server_Error"],
                code: ResultCode.InternalError));
        }
        
        //[Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _CountryService.Delete(id);
            if (result)
            {
                return Ok(ApiResponse<object>.SuccessResponse(
                    message: _localizer["Country_Deleted"],
                    code: ResultCode.Success));
            }

            return NotFound(ApiResponse<object>.FailureResponse(
                message: _localizer["Country_Not_Found"],
                code: ResultCode.NotFound));
        }
    }
}
