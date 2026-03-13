using Microsoft.AspNetCore.Mvc;
using WMS.Domain.Entities;
using WMS.Application.Interfaces;
using Microsoft.Extensions.Localization;
using WMS.Presentation.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Writers;
using System.ComponentModel.DataAnnotations;
namespace WMS.Presentation.Controllers
{
    [Route("api/Categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService,IStringLocalizer<SharedResource> localizer,IMapper mapper)
        {
            _categoryService = categoryService;
            _localizer = localizer;
            _mapper = mapper;
        }

        [HttpGet("All")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<CategoryDto> categories = await _categoryService.GetAll();

            if(!categories.Any())
                return NotFound(ApiResponse<object>.FailureResponse(_localizer["No_Categories_Found"]));

            return Ok(ApiResponse<IEnumerable<CategoryDto>>.SuccessResponse(
                data: categories,
                message: _localizer["Categories_Found"],
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

            CategoryDto categoryDto = await _categoryService.GetByID(id);

            if (categoryDto == null)
                return NotFound(ApiResponse<object>.FailureResponse(
                    message: _localizer["Category_Not_Found"],
                    code: ResultCode.NotFound));

            return Ok(ApiResponse<CategoryDto>.SuccessResponse(
                data: categoryDto,
                message: _localizer["Category_Found"],
                code: ResultCode.Success));
        }

        [Authorize]
        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add(CategoryDto categoryDto)
        {
            Category category = _mapper.Map<Category>(categoryDto);

            bool IsExist = await _categoryService.IsExistByName(category.CategoryName);

            if (IsExist)
                return BadRequest(ApiResponse<object>.FailureResponse(
                    message: _localizer["Category_Already_Exist"],
                    code: ResultCode.AlreadyExists));

            bool IsAdded = await _categoryService.AddNew(category);

            if(!IsAdded)
                return StatusCode(500, ApiResponse<object>.FailureResponse(
                message: _localizer["Server_Error"]));

            return CreatedAtAction(
                nameof(GetById), 
                new { id = categoryDto.CategoryId },
                ApiResponse<CategoryDto>.SuccessResponse(
                    data: categoryDto,
                    message: _localizer["Category_Created"],
                    code: ResultCode.Success)); 
        }
        [Authorize]
        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody]CategoryDto RequestCategoryDto)
        {
            Category category = _mapper.Map<Category>(RequestCategoryDto);

            bool IsUpdated = await _categoryService.Update(category);

            if (!IsUpdated)
                return NotFound(ApiResponse<object>.FailureResponse(
                    message: _localizer["Category_Not_Found"],
                    code: ResultCode.NotFound));

            return Ok(ApiResponse<object>.SuccessResponse(
                message: _localizer["Category_Updated"],
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

            var IsExist = await _categoryService.Delete(id);

            if(!IsExist)
                return NotFound(ApiResponse<object>.FailureResponse(
                    message: _localizer["Category_Not_Found"],
                    code: ResultCode.NotFound));

            return Ok(ApiResponse<object>.SuccessResponse(
                message: _localizer["Category_Deleted"],
                code: ResultCode.Success));
        }

    }
}
