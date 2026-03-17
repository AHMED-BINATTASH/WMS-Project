using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;
using WMS.Application.DTOs.WMS.Application.DTOs;
using WMS.Application.Interfaces;
using WMS.Application.Services;
using WMS.Domain.Entities;
using WMS.Presentation.Utilities;

namespace WMS.Presentation.Controllers
{
    [Route("api/Items")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly ICategoryService _categoryService;
        private readonly IUnitService _unitService;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IMapper _mapper;

        public ItemController(IItemService itemService, ICategoryService CategoryService, IUnitService UnitService,
            IStringLocalizer<SharedResource> localizer, IMapper mapper)
        {
            _itemService = itemService;
            _categoryService = CategoryService;
            _unitService = UnitService;
            _localizer = localizer;
            _mapper = mapper;
        }

        [HttpGet("All")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<ItemDto> items = await _itemService.GetAll();

            if (!items.Any())
                return NotFound(ApiResponse<object>.FailureResponse(_localizer["No_Items_Found"]));

            return Ok(ApiResponse<IEnumerable<ItemDto>>.SuccessResponse(
                data: items,
                message: _localizer["Items_Found"],
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

            ItemDto itemDto = await _itemService.GetByID(id);

            if (itemDto == null)
                return NotFound(ApiResponse<object>.FailureResponse(
                    message: _localizer["Item_Not_Found"],
                    code: ResultCode.NotFound));

            return Ok(ApiResponse<ItemDto>.SuccessResponse(
                data: itemDto,
                message: _localizer["Item_Found"],
                code: ResultCode.Success));
        }

        [Authorize]
        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status201Created)] 
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add(ItemDto itemDto)
        {
            Item item = _mapper.Map<Item>(itemDto);

            bool IsExist = await _itemService.IsExistByName(item.ItemName);

            if (IsExist)
                return BadRequest(ApiResponse<object>.FailureResponse(
                    message: _localizer["Item_Already_Exist"],
                    code: ResultCode.AlreadyExists));

            //var Category = await _categoryService.GetByID(item.CategoryID);
            //var Unit = await _unitService.GetByID(item.UnitID);

            //item.CategoryInfo = _mapper.Map<Category>(Category);
            //item.UnitInfo = _mapper.Map<Unit>(Unit);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            item.CreatedBy = int.Parse(userIdClaim ?? "0");
            item.CreatedAt = DateTime.UtcNow;
            bool IsAdded = await _itemService.AddNew(item);

            if (!IsAdded)
                return StatusCode(500, ApiResponse<object>.FailureResponse(
                    message: _localizer["Server_Error"]));

            return CreatedAtAction(
                nameof(GetById),
                new { id = item.ItemID }, 
                ApiResponse<ItemDto>.SuccessResponse(
                    data: itemDto,
                    message: _localizer["Item_Created"],
                    code: ResultCode.Success));
        }

        [Authorize]
        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] ItemDto RequestItemDto)
        {
            Item item = _mapper.Map<Item>(RequestItemDto);

            var ItemFromdb = await _itemService.GetByID(item.ItemID);

            if(ItemFromdb == null)
            {
                return BadRequest(ApiResponse<object>.FailureResponse(
                    message: _localizer["Invalid_Request"],
                    code: ResultCode.InvalidRequest));
            }

            if(ItemFromdb.ItemID == item.ItemID &&
               ItemFromdb.UnitID == item.UnitID &&
               ItemFromdb.IsActive == item.IsActive &&
               ItemFromdb.ItemName == item.ItemName &&
               ItemFromdb.Barcode == item.Barcode &&
               ItemFromdb.CategoryID == item.CategoryID)
            {
                return Ok(ApiResponse<object>.SuccessResponse(
                message: _localizer["Item_Updated"],
                code: ResultCode.Success));
            }
                

            bool IsUpdated = await _itemService.Update(item);

            if (!IsUpdated)
                return NotFound(ApiResponse<object>.FailureResponse(
                    message: _localizer["Item_Not_Found"],
                    code: ResultCode.NotFound));

            return Ok(ApiResponse<object>.SuccessResponse(
                message: _localizer["Item_Updated"],
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

            var IsExist = await _itemService.Delete(id);

            if (!IsExist)
                return NotFound(ApiResponse<object>.FailureResponse(
                    message: _localizer["Item_Not_Found"],
                    code: ResultCode.NotFound));

            return Ok(ApiResponse<object>.SuccessResponse(
                message: _localizer["Item_Deleted"],
                code: ResultCode.Success));
        }
    }
}
