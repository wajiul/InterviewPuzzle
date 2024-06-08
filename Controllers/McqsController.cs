using AutoMapper;
using InterviewPuzzle.Controllers.DTO;
using InterviewPuzzle.Data_Access;
using InterviewPuzzle.Data_Access.Context;
using InterviewPuzzle.Data_Access.Model;
using InterviewPuzzle.Data_Access.Repository;
using InterviewPuzzle.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InterviewPuzzle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class McqsController : ControllerBase
    {
        private readonly McqRepository _mcqRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public McqsController(McqRepository mcqRepository, IMapper mapper, IUnitOfWork uow)
        {
            _mcqRepository = mcqRepository;
            _mapper = mapper;
            _uow = uow;
        }

        /// <summary>
        /// Gets all MCQs.
        /// </summary>
        /// <returns>Returns a list of all MCQs.</returns>
        /// <response code="200">Returns the list of MCQs.</response>
        [HttpGet]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<MCQ>>), 200)]
        public async Task<IActionResult> Get()
        {
            var mcqs = await _mcqRepository.GetAllMcqAsync();
            return Ok(new APIResponse<IEnumerable<MCQ>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = mcqs
            });
        }

        /// <summary>
        /// Gets an MCQ by ID.
        /// </summary>
        /// <param name="id">The ID of the MCQ.</param>
        /// <returns>Returns the MCQ with the specified ID.</returns>
        /// <response code="200">Returns the MCQ with the specified ID.</response>
        /// <response code="404">If the MCQ with the specified ID is not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(APIResponse<MCQ>), 200)]
        public async Task<IActionResult> Get(int id)
        {
            var mcq = await _mcqRepository.GetMcqAsync(id);
            if (mcq == null)
            {
                return NotFound(new APIResponse<string>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorMessages = new List<string> { $"MCQ with Id {id} not found" }
                });
            }

            return Ok(new APIResponse<MCQ>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = mcq
            });
        }

        /// <summary>
        /// Gets MCQs by course name.
        /// </summary>
        /// <param name="course">The course name.</param>
        /// <returns>Returns a list of MCQs for the specified course.</returns>
        /// <response code="200">Returns the list of MCQs for the specified course.</response>
        /// <response code="404">If no MCQs are found for the specified course.</response>
        [HttpGet("bycourse")]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<MCQ>>), 200)]
        public async Task<IActionResult> GetMcqsByCourse([FromQuery] string course)
        {
            var mcqs = await _mcqRepository.GetMcqByCourseAsync(course.ToLower());
            if (mcqs == null || mcqs.Count == 0)
            {
                return NotFound(new APIResponse<string>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorMessages = new List<string> { $"MCQ with course '{course}' not found" }
                });
            }

            return Ok(new APIResponse<IEnumerable<MCQ>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = mcqs
            });
        }
    }
}
