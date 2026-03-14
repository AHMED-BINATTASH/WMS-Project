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
    [Route("api/ItemUnits")]
    [ApiController]
    public class ItemUnitController : ControllerBase
    {
        private readonly IService<ItemUnitDto, ItemUnit> _itemUnitService;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IMapper _mapper;

        public ItemUnitController(IService<ItemUnitDto,ItemUnit> itemUnitService, IStringLocalizer<SharedResource> localizer, IMapper mapper)
        {
            _itemUnitService = itemUnitService;
            _localizer = localizer;
            _mapper = mapper;
        }

        [HttpGet("All")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<ItemUnitDto> itemUnits = await _itemUnitService.GetAll();

            if (!itemUnits.Any())
                return NotFound(ApiResponse<object>.FailureResponse(_localizer["No_ItemUnits_Found"]));

            return Ok(ApiResponse<IEnumerable<ItemUnitDto>>.SuccessResponse(
                data: itemUnits,
                message: _localizer["ItemUnits_Found"],
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

            ItemUnitDto itemUnitDto = await _itemUnitService.GetByID(id);

            if (itemUnitDto == null)
                return NotFound(ApiResponse<object>.FailureResponse(
                    message: _localizer["ItemUnit_Not_Found"],
                    code: ResultCode.NotFound));

            return Ok(ApiResponse<ItemUnitDto>.SuccessResponse(
                data: itemUnitDto,
                message: _localizer["ItemUnit_Found"],
                code: ResultCode.Success));
        }

        [Authorize]
        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add(ItemUnitDto itemUnitDto)
        {
            ItemUnit itemUnit = _mapper.Map<ItemUnit>(itemUnitDto);

            bool IsAdded = await _itemUnitService.AddNew(itemUnit);

            if (!IsAdded)
                return StatusCode(500, ApiResponse<object>.FailureResponse(
                    message: _localizer["Server_Error"]));

            return CreatedAtAction(
                nameof(GetById),
                new { id = itemUnit.ItemUnitID }, 
                ApiResponse<ItemUnitDto>.SuccessResponse(
                    data: itemUnitDto,
                    message: _localizer["ItemUnit_Created"],
                    code: ResultCode.Success));
        }

        [Authorize]
        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] ItemUnitDto RequestItemUnitDto)
        {
            ItemUnit itemUnit = _mapper.Map<ItemUnit>(RequestItemUnitDto);

            bool IsUpdated = await _itemUnitService.Update(itemUnit);

            if (!IsUpdated)
                return NotFound(ApiResponse<object>.FailureResponse(
                    message: _localizer["ItemUnit_Not_Found"],
                    code: ResultCode.NotFound));

            return Ok(ApiResponse<object>.SuccessResponse(
                message: _localizer["ItemUnit_Updated"],
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

            var IsExist = await _itemUnitService.Delete(id);

            if (!IsExist)
                return NotFound(ApiResponse<object>.FailureResponse(
                    message: _localizer["ItemUnit_Not_Found"],
                    code: ResultCode.NotFound));

            return Ok(ApiResponse<object>.SuccessResponse(
                message: _localizer["ItemUnit_Deleted"],
                code: ResultCode.Success));
        }
    }
}
