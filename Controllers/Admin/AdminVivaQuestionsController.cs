using AutoMapper;
using InterviewPuzzle.Data_Access.Repository;
using InterviewPuzzle.Data_Access;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InterviewPuzzle.Controllers.DTO;
using InterviewPuzzle.Data_Access.Model;
using InterviewPuzzle.Exceptions;
using System.Net;
using System.Net.Mime;

namespace InterviewPuzzle.Controllers.Admin
{
    /// <summary>
    /// Controller for managing Viva Questions.
    /// </summary>
    
    
    [Route("api/admin/interviews")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize(Roles = "admin")]
    public class AdminVivaQuestionsController : ControllerBase
    {
        private readonly VivaQuestionRepository _questionRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public AdminVivaQuestionsController(VivaQuestionRepository questionRepository, IMapper mapper, IUnitOfWork uow)
        {
            _questionRepository = questionRepository;
            _mapper = mapper;
            _uow = uow;
        }

        /// <summary>
        /// Adds a new Viva question.
        /// </summary>
        /// <param name="questionDto">The DTO containing the Viva question data.</param>
        /// <returns>A response indicating the result of the add operation.</returns>
        /// <response code="200">Returns the created question.</response>
        /// <response code="400">Returns error message if the question already exists.</response>
        /// <remarks>
        /// This endpoint requires authentication with the admin role.
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(APIResponse<VivaQuestion>), 200)]
        [ProducesResponseType(typeof(APIResponse<string>), 400)]
        public async Task<IActionResult> Add([FromBody] VivaQuestionDto questionDto)
        {
            var exist = await _questionRepository.isQuestionExist(questionDto.Coursename, questionDto.Text);
            if (exist)
            {
                var response = new APIResponse<string>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = new List<string> { "Question already exists" }
                };
                return BadRequest(response);
            }

            var question = _mapper.Map<VivaQuestionDto, VivaQuestion>(questionDto);

            await _questionRepository.AddVivaQuestionAsync(question);
            await _uow.Complete();

            var createdResponse = new APIResponse<VivaQuestion>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.Created,
                Result = question
            };

            return Ok(createdResponse);
        }

        /// <summary>
        /// Adds a list of Viva questions.
        /// </summary>
        /// <param name="questionList">The list of Viva question DTOs to add.</param>
        /// <returns>A response indicating the result of the bulk add operation.</returns>
        /// <response code="200">Returns a success message indicating the result of the operation.</response>
        /// <remarks>
        /// This endpoint requires authentication with the admin role.
        /// </remarks>
        [HttpPost("bulk")]
        [ProducesResponseType(typeof(APIResponse<string>), 200)]
        public async Task<IActionResult> AddQuestionList([FromBody] List<VivaQuestionDto> questionList)
        {
            int failedToAdd = 0;
            foreach (var questionDto in questionList)
            {
                var exist = await _questionRepository.isQuestionExist(questionDto.Coursename, questionDto.Text);
                if (exist)
                {
                    failedToAdd++;
                    continue;
                }

                var question = _mapper.Map<VivaQuestionDto, VivaQuestion>(questionDto);
                await _questionRepository.AddVivaQuestionAsync(question);
            }
            await _uow.Complete();

            var response = new APIResponse<string>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = failedToAdd > 0
                    ? $"{failedToAdd} Viva questions already exist and were not added to the database"
                    : "All viva questions added successfully"
            };

            return Ok(response);
        }

        /// <summary>
        /// Updates an existing Viva question.
        /// </summary>
        /// <param name="id">The ID of the Viva question to update.</param>
        /// <param name="questionDto">The DTO containing the updated Viva question data.</param>
        /// <returns>A response indicating the result of the update operation.</returns>
        /// <response code="200">Returns a success message of Viva question added successfully.</response>
        /// <response code="404">Returns a error message of Viva question with ID does not exist.</response>
        /// <remarks>
        /// This endpoint requires authentication with the admin role.
        /// </remarks>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(APIResponse<string>), 200)]
        public async Task<IActionResult> Update(int id, [FromBody] VivaQuestionDto questionDto)
        {
            var question = await _questionRepository.GetVivaQuestionAsync(id);
            if (question == null)
            {
                var response = new APIResponse<string>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorMessages = new List<string> { $"Viva question with Id {id} not found" }
                };
                return NotFound(response);
            }

            var newQuestion = _mapper.Map(questionDto, question);

            _questionRepository.UpdateVivaQuestion(newQuestion);
            await _uow.Complete();

            var successResponse = new APIResponse<string>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = "Updated successfully"
            };

            return Ok(successResponse);
        }

        /// <summary>
        /// Deletes an existing Viva question.
        /// </summary>
        /// <param name="id">The ID of the Viva question to delete.</param>
        /// <returns>A response indicating the result of the delete operation.</returns>
        /// <response code="200">Returns a success message of Viva question added successfully.</response>
        /// <response code="404">Returns a error message of Viva question with ID does not exist.</response>
        /// <remarks>
        /// This endpoint requires authentication with the admin role.
        /// </remarks>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(APIResponse<string>), 200)]

        public async Task<IActionResult> Delete(int id)
        {
            var question = await _questionRepository.GetVivaQuestionAsync(id);
            if (question == null)
            {
                var response = new APIResponse<string>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorMessages = new List<string> { $"Viva question with Id {id} not found" }
                };
                return NotFound(response);
            }

            _questionRepository.DeleteVivaQuestion(question);
            await _uow.Complete();

            var successResponse = new APIResponse<string>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = "Deleted successfully"
            };

            return Ok(successResponse);
        }        
    }
}
