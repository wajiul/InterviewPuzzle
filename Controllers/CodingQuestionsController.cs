using AutoMapper;
using InterviewPuzzle.Data_Access.Context;
using InterviewPuzzle.Data_Access.Repository;
using InterviewPuzzle.Data_Access;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InterviewPuzzle.Controllers.DTO;
using InterviewPuzzle.Exceptions;
using InterviewPuzzle.Data_Access.Model;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Mime;

namespace InterviewPuzzle.Controllers
{
    /// <summary>
    /// Controller for managing coding questions.
    /// </summary>
    [Route("api/coding")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class CodingQuestionsController : ControllerBase
    {
        private readonly CodingQuestionRepository _questionRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        public CodingQuestionsController(InterviewPuzzleDbContext context, CodingQuestionRepository questionRepository, IMapper mapper, IUnitOfWork uow)
        {
            _questionRepository = questionRepository;
            _mapper = mapper;
            _uow = uow;
        }

        /// <summary>
        /// Gets all coding questions.
        /// </summary>
        /// <returns>A list of all coding questions.</returns>
        /// <response code="200">Returns the list of coding questions.</response>
        [HttpGet]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<CodingQuestion>>), 200)]
        public async Task<IActionResult> Get()
        {
            var questions = await _questionRepository.GetCodingQuestionsAsync();
            return Ok(new APIResponse<IEnumerable<CodingQuestion>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = questions
            });
        }

        /// <summary>
        /// Gets a specific coding question by ID.
        /// </summary>
        /// <param name="id">The ID of the coding question.</param>
        /// <returns>The requested coding question.</returns>
        /// <response code="200">Returns the coding question.</response>
        /// <response code="404">If the coding question is not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(APIResponse<CodingQuestion>), 200)]
        public async Task<IActionResult> Get(int id)
        {
            var question = await _questionRepository.GetCodingQuestionAsync(id);
            if (question == null)
            {
                return NotFound(new APIResponse<string>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorMessages = new List<string> { $"Coding question with ID {id} not found" }
                });
            }

            return Ok(new APIResponse<CodingQuestion>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = question
            });
        }

        /// <summary>
        /// Gets coding questions by tag.
        /// </summary>
        /// <param name="tag">The tag of the coding questions.</param>
        /// <returns>A list of coding questions with the specified tag.</returns>
        /// <response code="200">Returns the list of coding questions with the specified tag.</response>
        /// <response code="404">If no coding questions with the specified tag are found.</response>
        [HttpGet("bytag")]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<CodingQuestion>>), 200)]
        public async Task<IActionResult> GetQuestionByTag([FromQuery] string tag)
        {
            var questions = await _questionRepository.GetCodingQuestionsByTagAsync(tag.ToLower());
            if (questions == null || !questions.Any())
            {
                return NotFound(new APIResponse<string>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorMessages = new List<string> { $"Coding question with tag '{tag}' not found" }
                });
            }

            return Ok(new APIResponse<IEnumerable<CodingQuestion>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = questions
            });
        }
    }
}
