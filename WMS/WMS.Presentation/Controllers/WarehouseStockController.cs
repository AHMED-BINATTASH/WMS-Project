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
    [Route("api/WarehouseStocks")]
    [ApiController]
    public class WarehouseStockController : ControllerBase
    {
        private readonly IWarehouseStockService _warehouseStockService;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IMapper _mapper;

        public WarehouseStockController(IWarehouseStockService warehouseStockService, IStringLocalizer<SharedResource> localizer, IMapper mapper)
        {
            _warehouseStockService = warehouseStockService;
            _localizer = localizer;
            _mapper = mapper;
        }

        [HttpGet("All")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<WarehouseStockDto> warehouseStocks = await _warehouseStockService.GetAll();

            if (!warehouseStocks.Any())
                return NotFound(ApiResponse<object>.FailureResponse(_localizer["No_WarehouseStocks_Found"]));

            return Ok(ApiResponse<IEnumerable<WarehouseStockDto>>.SuccessResponse(
                data: warehouseStocks,
                message: _localizer["WarehouseStocks_Found"],
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

            WarehouseStockDto warehouseStockDto = await _warehouseStockService.GetByID(id);

            if (warehouseStockDto == null)
                return NotFound(ApiResponse<object>.FailureResponse(
                    message: _localizer["WarehouseStock_Not_Found"],
                    code: ResultCode.NotFound));

            return Ok(ApiResponse<WarehouseStockDto>.SuccessResponse(
                data: warehouseStockDto,
                message: _localizer["WarehouseStock_Found"],
                code: ResultCode.Success));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add(WarehouseStockDto warehouseStockDto)
        {
            WarehouseStock warehouseStock = _mapper.Map<WarehouseStock>(warehouseStockDto);

            // التحقق من عدم وجود نفس الصنف في نفس المستودع مسبقاً
            // إذا كان موجود، المفروض نسوي Update للكمية مو Add جديد!
            bool IsExist = await _warehouseStockService.IsExistCombination(warehouseStock.WarehouseID, warehouseStock.ItemID);

            if (IsExist)
                return BadRequest(ApiResponse<object>.FailureResponse(
                    message: _localizer["WarehouseStock_Already_Exist"],
                    code: ResultCode.AlreadyExists));

            bool IsAdded = await _warehouseStockService.AddNew(warehouseStock);

            if (!IsAdded)
                return StatusCode(500, ApiResponse<object>.FailureResponse(
                    message: _localizer["Server_Error"]));

            return CreatedAtAction(
                nameof(GetById),
                new { id = warehouseStock.WarehouseStockID },
                ApiResponse<WarehouseStockDto>.SuccessResponse(
                    data: warehouseStockDto,
                    message: _localizer["WarehouseStock_Created"],
                    code: ResultCode.Success));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] WarehouseStockDto RequestWarehouseStockDto)
        {
            WarehouseStock warehouseStock = _mapper.Map<WarehouseStock>(RequestWarehouseStockDto);

            bool IsUpdated = await _warehouseStockService.Update(warehouseStock);

            if (!IsUpdated)
                return NotFound(ApiResponse<object>.FailureResponse(
                    message: _localizer["WarehouseStock_Not_Found"],
                    code: ResultCode.NotFound));

            return Ok(ApiResponse<object>.SuccessResponse(
                message: _localizer["WarehouseStock_Updated"],
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

            var IsDeleted = await _warehouseStockService.Delete(id);

            if (!IsDeleted)
                return NotFound(ApiResponse<object>.FailureResponse(
                    message: _localizer["WarehouseStock_Not_Found"],
                    code: ResultCode.NotFound));

            return Ok(ApiResponse<object>.SuccessResponse(
                message: _localizer["WarehouseStock_Deleted"],
                code: ResultCode.Success));
        }
    }
}
