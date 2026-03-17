using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WMS.Application.DTOs;
using WMS.Application.Interfaces;
using WMS.Application.Services;
using WMS.Domain.Entities;
using WMS.Presentation.Utilities;

namespace WMS.Presentation.Controllers
{
    [Route("api/Units")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        private readonly IUnitService _unitService;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IMapper _mapper;

        public UnitController(IUnitService unitService, IStringLocalizer<SharedResource> localizer, IMapper mapper)
        {
            _unitService = unitService;
            _localizer = localizer;
            _mapper = mapper;
        }

        [HttpGet("All")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<UnitDto> units = await _unitService.GetAll();

            if (!units.Any())
                return NotFound(ApiResponse<object>.FailureResponse(_localizer["No_Units_Found"]));

            return Ok(ApiResponse<IEnumerable<UnitDto>>.SuccessResponse(
                data: units,
                message: _localizer["Units_Found"],
                code: ResultCode.Success));
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(int id)
        {
            if (id < 1)
                return BadRequest(ApiResponse<object>.FailureResponse(
                    message: _localizer["Invalid_ID"],
                    code: ResultCode.InvalidRequest));

            UnitDto unitDto = await _unitService.GetByID(id);

            if (unitDto == null)
                return NotFound(ApiResponse<object>.FailureResponse(
                    message: _localizer["Unit_Not_Found"],
                    code: ResultCode.NotFound));

            return Ok(ApiResponse<UnitDto>.SuccessResponse(
                data: unitDto,
                message: _localizer["Unit_Found"],
                code: ResultCode.Success));
        }

        [Authorize]
        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add(UnitDto unitDto)
        {
            Unit unit = _mapper.Map<Unit>(unitDto);

            bool IsExist = await _unitService.IsExistByName(unit.UnitName);

            if (IsExist)
                return BadRequest(ApiResponse<object>.FailureResponse(
                    message: _localizer["Unit_Already_Exist"],
                    code: ResultCode.AlreadyExists));

            bool IsAdded = await _unitService.AddNew(unit);

            if (!IsAdded)
                return StatusCode(500, ApiResponse<object>.FailureResponse(
                    message: _localizer["Server_Error"]));

            return CreatedAtAction(
                nameof(GetById),
                new { id = unit.UnitID },
                ApiResponse<UnitDto>.SuccessResponse(
                    data: unitDto,
                    message: _localizer["Unit_Created"],
                    code: ResultCode.Success));
        }

        [Authorize]
        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] UnitDto RequestUnitDto)
        {
            Unit unit = _mapper.Map<Unit>(RequestUnitDto);

            var UnitFromDB = await _unitService.GetByID(unit.UnitID);

            if (UnitFromDB == null)
            {
                return BadRequest(ApiResponse<object>.FailureResponse(
                    message: _localizer["Invalid_Request"],
                    code: ResultCode.InvalidRequest));
            }

            if(UnitFromDB.UnitID == unit.UnitID &&
                UnitFromDB.UnitName == unit.UnitName &&
                UnitFromDB.UnitSymbol == unit.UnitSymbol)
            {
                return Ok(ApiResponse<object>.SuccessResponse(
                message: _localizer["Unit_Updated"],
                code: ResultCode.Success));
            }

            bool IsUpdated = await _unitService.Update(unit);

            if (!IsUpdated)
                return NotFound(ApiResponse<object>.FailureResponse(
                    message: _localizer["Unit_Not_Found"],
                    code: ResultCode.NotFound));

            return Ok(ApiResponse<object>.SuccessResponse(
                message: _localizer["Unit_Updated"],
                code: ResultCode.Success));
        }

        [Authorize]
        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 1)
                return BadRequest(ApiResponse<object>.FailureResponse(
                    message: _localizer["Invalid_ID"],
                    code: ResultCode.InvalidRequest));

            var IsExist = await _unitService.Delete(id);

            if (!IsExist)
                return NotFound(ApiResponse<object>.FailureResponse(
                    message: _localizer["Unit_Not_Found"],
                    code: ResultCode.NotFound));

            return Ok(ApiResponse<object>.SuccessResponse(
                message: _localizer["Unit_Deleted"],
                code: ResultCode.Success));
        }
    }
}
