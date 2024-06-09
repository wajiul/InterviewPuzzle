using AutoMapper;
using InterviewPuzzle.Controllers.DTO;
using InterviewPuzzle.Data_Access;
using InterviewPuzzle.Data_Access.Model;
using InterviewPuzzle.Data_Access.Repository;
using InterviewPuzzle.Exceptions;
using InterviewPuzzle.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InterviewPuzzle.Controllers.Admin
{
    [Route("api/admin/coding")]
    [ApiController]
    [Authorize(Roles = "admin")]
    [ValidateModel]
    public class AdminCodingQuestionsController : ControllerBase
    {
        private readonly CodingQuestionRepository _questionRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        public AdminCodingQuestionsController(CodingQuestionRepository questionRepository,IMapper mapper, IUnitOfWork uow)
        {
            _questionRepository = questionRepository;
            _mapper = mapper;
            _uow = uow;
        }

        /// <summary>
        /// Adds a new coding question to the database
        /// </summary>
        /// <param name="questionDto">The DTO containing the coding question details.</param>
        /// <returns>The created coding problem's details.</returns>
        /// <exception cref="AlreadyExistException">T</exception>
        /// <remarks>
        /// This endpoint requires authentication with the admin role.
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(APIResponse<string>),200)]
        public async Task<IActionResult> Add([FromBody] CodingQuestionDto questionDto)
        {
            var response = new APIResponse<string>();
            var exist = await _questionRepository.IsQuestionExist(questionDto.Text);
            if (exist)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.ErrorMessages.Add("Coding question already exist");
                return BadRequest(response);
            }

            var question = _mapper.Map<CodingQuestionDto, CodingQuestion>(questionDto);

            _questionRepository.Add(question);
            await _uow.Complete();

            response.StatusCode = HttpStatusCode.OK;
            response.Result = "Coding question added successfully";

            return Ok(response);
        }
        /// <summary>
        /// Updates an existing coding question.
        /// </summary>
        /// <param name="id">The ID of the coding question to update.</param>
        /// <param name="questionDto">The DTO containing the updated data for the coding question.</param>
        /// <returns>A response indicating the result of the update operation.</returns>
        /// <response code="200">Returns a success message if the update was successful.</response>
        /// <response code="404">Returns an error message if the coding question was not found.</response>
        /// <remarks>
        /// This endpoint requires authentication with the admin role.
        /// </remarks>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(APIResponse<string>), 200)]
        [ValidateModel]
        public async Task<IActionResult> Update(int id, [FromBody] CodingQuestionDto questionDto)
        {
            
            var response = new APIResponse<string>();

            var question = await _questionRepository.GetCodingQuestionAsync(id);
            if (question == null)
            {
                return NotFound(new APIResponse<string>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = new List<string> { $"Coding question with ID {id} not found." }
                });
            }

            var updatedQuestion = _mapper.Map<CodingQuestionDto, CodingQuestion>(questionDto, question);

            _questionRepository.UpdateCodingQuestion(updatedQuestion);
            await _uow.Complete();
            return Ok(new APIResponse<string>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = "Updated successfully"
            });
        }

        /// <summary>
        /// Deletes a coding question by its ID.
        /// </summary>
        /// <param name="id">The ID of the coding question to be deleted.</param>
        /// <returns>A response indicating the result of the delete operation.</returns>
        /// <response code="200">Returns a success message if the coding question was deleted successfully.</response>
        /// <response code="404">Returns an error message if the coding question was not found.</response>
        /// <remarks>
        /// This endpoint requires authentication with the admin role.
        /// </remarks>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(APIResponse<string>), 200)]
        public async Task<IActionResult> Delete(int id)
        {
            var question = await _questionRepository.GetCodingQuestionAsync(id);
            if(question == null)
            {
                return NotFound(new APIResponse<string>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = new List<string> { $"Coding question with ID {id} not found." }
                });
            }
            _questionRepository.DeleteCodingQuestion(question);
            await _uow.Complete();

            return Ok(new APIResponse<string>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = "Deleted successfully"
            });
        }

        /// <summary>
        /// Deletes a solution from a specific coding question.
        /// </summary>
        /// <param name="questionId">The ID of the coding question from which to delete the solution.</param>
        /// <param name="solutionId">The ID of the solution to be deleted.</param>
        /// <returns>A response indicating the result of the delete operation.</returns>
        /// <response code="200">Returns a success message if the solution was deleted successfully.</response>
        /// <response code="404">Returns an error message if the coding question or solution was not found.</response>
        /// <remarks>
        /// This endpoint requires authentication with the admin role.
        /// </remarks>
        [HttpDelete("codingQuestions/{questionId}/solution/{solutionId}")]
        [ProducesResponseType(typeof(APIResponse<string>), 200)]
        public async Task<IActionResult> DeleteOption(int questionId, int solutionId)
        {
            var question = await _questionRepository.GetCodingQuestionAsync(questionId);
            if (question == null)
            {
                return NotFound(new APIResponse<string>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorMessages = new List<string> { $"Coding question with ID {questionId} not found." }
                });
            }

            var solution = question.Solutions.FirstOrDefault(s => s.Id == solutionId);
            if (solution == null)
            {
                return NotFound(new APIResponse<string>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorMessages = new List<string> { $"Solution with ID {solutionId} not found in question ID {questionId}." }
                });
            }

            await _questionRepository.DeleteSolution(questionId, solutionId);
            await _uow.Complete();

            return Ok(new APIResponse<string>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = $"Solution ID {solutionId} removed from question {questionId}."
            });
        }

    }
}
