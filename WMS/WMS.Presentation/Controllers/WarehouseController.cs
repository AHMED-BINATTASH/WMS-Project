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
    [Route("api/Warehouses")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IMapper _mapper;

        public WarehouseController(IWarehouseService warehouseService, IStringLocalizer<SharedResource> localizer, IMapper mapper)
        {
            _warehouseService = warehouseService;
            _localizer = localizer;
            _mapper = mapper;
        }

        [HttpGet("All")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<WarehouseDto> warehouses = await _warehouseService.GetAll();

            if (!warehouses.Any())
                return NotFound(ApiResponse<object>.FailureResponse(_localizer["No_Warehouses_Found"]));

            return Ok(ApiResponse<IEnumerable<WarehouseDto>>.SuccessResponse(
                data: warehouses,
                message: _localizer["Warehouses_Found"],
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

            WarehouseDto warehouseDto = await _warehouseService.GetByID(id);

            if (warehouseDto == null)
                return NotFound(ApiResponse<object>.FailureResponse(
                    message: _localizer["Warehouse_Not_Found"],
                    code: ResultCode.NotFound));

            return Ok(ApiResponse<WarehouseDto>.SuccessResponse(
                data: warehouseDto,
                message: _localizer["Warehouse_Found"],
                code: ResultCode.Success));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add(WarehouseDto warehouseDto)
        {
            Warehouse warehouse = _mapper.Map<Warehouse>(warehouseDto);

            bool IsExist = await _warehouseService.IsExistByName(warehouse.WarehouseName);

            if (IsExist)
                return BadRequest(ApiResponse<object>.FailureResponse(
                    message: _localizer["Warehouse_Already_Exist"],
                    code: ResultCode.AlreadyExists));

            bool IsAdded = await _warehouseService.AddNew(warehouse);

            if (!IsAdded)
                return StatusCode(500, ApiResponse<object>.FailureResponse(
                    message: _localizer["Server_Error"]));

            return CreatedAtAction(
                nameof(GetById),
                new { id = warehouse.WarehouseID }, 
                ApiResponse<WarehouseDto>.SuccessResponse(
                    data: warehouseDto,
                    message: _localizer["Warehouse_Created"],
                    code: ResultCode.Success));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] WarehouseDto RequestWarehouseDto)
        {
            Warehouse warehouse = _mapper.Map<Warehouse>(RequestWarehouseDto);

            bool IsUpdated = await _warehouseService.Update(warehouse);

            if (!IsUpdated)
                return NotFound(ApiResponse<object>.FailureResponse(
                    message: _localizer["Warehouse_Not_Found"],
                    code: ResultCode.NotFound));

            return Ok(ApiResponse<object>.SuccessResponse(
                message: _localizer["Warehouse_Updated"],
                code: ResultCode.Success));
        }

        [Authorize(Roles = "Admin")]
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

            var IsDeleted = await _warehouseService.Delete(id);

            if (!IsDeleted)
                return NotFound(ApiResponse<object>.FailureResponse(
                    message: _localizer["Warehouse_Not_Found"],
                    code: ResultCode.NotFound));

            return Ok(ApiResponse<object>.SuccessResponse(
                message: _localizer["Warehouse_Deleted"],
                code: ResultCode.Success));
        }
    }
}
