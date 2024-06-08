using AutoMapper;
using InterviewPuzzle.Data_Access.Context;
using InterviewPuzzle.Data_Access.Repository;
using InterviewPuzzle.Data_Access;
using Microsoft.AspNetCore.Mvc;
using InterviewPuzzle.Exceptions;
using InterviewPuzzle.Data_Access.Model;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using InterviewPuzzle.Controllers.DTO;

namespace InterviewPuzzle.Controllers
{
    /// <summary>
    /// Controller for managing viva questions.
    /// </summary>
    [Route("api/interviews")]
    [ApiController]
    public class VivaQuestionsController : ControllerBase
    {
        private readonly VivaQuestionRepository _questionRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public VivaQuestionsController(VivaQuestionRepository questionRepository, IMapper mapper, IUnitOfWork uow)
        {
            _questionRepository = questionRepository;
            _mapper = mapper;
            _uow = uow;
        }

        /// <summary>
        /// Retrieves all viva questions.
        /// </summary>
        /// <returns>A list of all viva questions.</returns>
        /// <response code="200">Returns the list of viva questions.</response>
        [HttpGet]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<VivaQuestion>>), 200)]
        public async Task<IActionResult> Get()
        {
            var questions = await _questionRepository.GetVivaQuestionsAsync();
            return Ok(new APIResponse<IEnumerable<VivaQuestion>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = questions   
            });
        }

        /// <summary>
        /// Retrieves a viva question by its ID.
        /// </summary>
        /// <param name="id">The ID of the viva question to retrieve.</param>
        /// <returns>The viva question with the specified ID.</returns>
        /// <response code="200">Returns the viva question with the specified ID.</response>
        /// <response code="404">If the viva question with the specified ID is not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(APIResponse<VivaQuestion>), 200)]
        public async Task<IActionResult> Get(int id)
        {
            var question = await _questionRepository.GetVivaQuestionAsync(id);
            if (question == null)
            {
                return NotFound(new APIResponse<string>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorMessages = new List<string> { $"Viva question with Id {id} not found" }
                });
            }
            return Ok(new APIResponse<VivaQuestion>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = question
            });
        }

        /// <summary>
        /// Retrieves viva questions by course.
        /// </summary>
        /// <param name="course">The course name.</param>
        /// <returns>A list of viva questions for the specified course.</returns>
        /// <response code="200">Returns the list of viva questions for the specified course.</response>
        /// <response code="404">If no viva questions are found for the specified course.</response>
        [HttpGet("bycourse")]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<VivaQuestion>>),200)]
        public async Task<IActionResult> GetQuestionsByCourse([FromQuery] string course)
        {
            var questions = await _questionRepository.GetQuestionByCourseAsync(course.ToLower());
            if (questions == null || !questions.Any())
            {
                return NotFound(new APIResponse<string>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorMessages = new List<string> { $"Viva questions with course '{course}' not found" }
                });
            }
            return Ok(new APIResponse<IEnumerable<VivaQuestion>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = questions
            });
        }
    }
}
