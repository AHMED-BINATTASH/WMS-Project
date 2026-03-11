//using AutoMapper;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Localization;
//using System.Diagnostics.Metrics;
//using WMS.Application.DTOs;
//using WMS.Application.Interfaces;
//using WMS.Application.Services;
//using WMS.Domain.Entities;
//using WMS.Presentation.Utilities;

//namespace WMS.Presentation.Controllers
//{
//    [Route("api/Person")]
//    [ApiController]
//    public class PersonController : ControllerBase
//    {
//        private readonly JWTSettings _jWTSettings;
//        private readonly IService<PersonDto, Person> _PersonService;
//        private readonly IStringLocalizer<SharedResource> _localizer;
//        private readonly IMapper _mapper;

//        public PersonController(JWTSettings jWTSettings, IService<PersonDto,Person> PersonService, IStringLocalizer<SharedResource> localizer, IMapper mapper)
//        {
//            _jWTSettings = jWTSettings;
//            _PersonService = PersonService;
//            _localizer = localizer;
//            _mapper = mapper;
//        }

//        [Authorize(Roles ="Admin")]
//        [HttpGet("All")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<IActionResult> GetAll()
//        {
//            IEnumerable<PersonDto> People = await _PersonService.GetAll();

//            if (People.Count() == 0)
//            {
//                return NotFound(ApiResponse<object>.FailureResponse(message: _localizer["NoPeopleFound"]));
//            }

//            return Ok(ApiResponse<IEnumerable<PersonDto>>.SuccessResponse(
//                data: People
//                ));
//        }

//        [Authorize]
//        [HttpGet("{id}")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<IActionResult> GetById(int id)
//        {
//            var Person = await _PersonService.GetByID(id);

//            if (Person == null)
//            {
//                return NotFound(ApiResponse<object>.FailureResponse(
//                    message: _localizer["PersonNotFound"],
//                    code: ResultCode.NotFound));
//            }

//            return Ok(ApiResponse<PersonDto>.SuccessResponse(data: Person));
//        }

//        [Authorize(Roles = "Admin")]
//        [HttpPost("Add")]
//        [ProducesResponseType(StatusCodes.Status201Created)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        public async Task<IActionResult> Add([FromBody] PersonDto PersonDto)
//        {
//            if (PersonDto == null)
//            {
//                return BadRequest(ApiResponse<object>.FailureResponse(
//                    message: _localizer["InvalidRequest"],
//                    code: ResultCode.InvalidRequest));
//            }

//            Person person = _mapper.Map<Person>(PersonDto);
//            // Assuming your service returns the created object or its ID
//            var IsAdded = await _PersonService.AddNew(person);

//            if (IsAdded)
//            {
//                PersonDto createdPersonDto = _mapper.Map<PersonDto>(person);

//                return CreatedAtAction(
//                    nameof(GetById),
//                    new { id = person.PersonID },
//                    ApiResponse<PersonDto>.SuccessResponse(
//                        data: createdPersonDto,
//                        message: _localizer["PersonCreated"],
//                        code: ResultCode.Success)
//                );
//            }

//            return StatusCode(500, ApiResponse<object>.FailureResponse(
//                message: _localizer["ServerError"]));
//        }

//        [Authorize(Roles = "Admin")]
//        [HttpPut("Update")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        public async Task<IActionResult> Update([FromBody] PersonDto PersonDto)
//        {
//            if (PersonDto == null)
//            {
//                return BadRequest(ApiResponse<object>.FailureResponse(message: _localizer["InvalidRequest"]));
//            }

//            Person person = _mapper.Map<Person>(PersonDto);

//            var result = await _PersonService.Update(person);
//            if (result)
//            {
//                return Ok(ApiResponse<object>.SuccessResponse(
//                    message: _localizer["PersonUpdated"],
//                    code: ResultCode.Success));
//            }

//            return NotFound(ApiResponse<object>.FailureResponse(message: _localizer["PersonNotFound"]));
//        }

//        [Authorize(Roles = "Admin")]
//        [HttpDelete("Delete/{id}")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var result = await _PersonService.Delete(id);
//            if (result)
//            {
//                return Ok(ApiResponse<object>.SuccessResponse(
//                    message: _localizer["PersonDeleted"],
//                    code: ResultCode.Success));
//            }

//            return NotFound(ApiResponse<object>.FailureResponse(
//                message: _localizer["PersonNotFound"],
//                code: ResultCode.NotFound));
//        }
//    }
//}
