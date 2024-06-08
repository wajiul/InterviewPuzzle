using AutoMapper;
using InterviewPuzzle.Data_Access.Repository;
using InterviewPuzzle.Data_Access;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InterviewPuzzle.Controllers.DTO;
using InterviewPuzzle.Data_Access.Model;
using InterviewPuzzle.Exceptions;
using System.Net;

namespace InterviewPuzzle.Controllers.Admin
{
    [Route("api/admin/mcqs")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminMcqsController : ControllerBase
    {
        private readonly McqRepository _mcqRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        public AdminMcqsController(McqRepository mcqRepository, IMapper mapper, IUnitOfWork uow)
        {
            _mcqRepository = mcqRepository;
            _mapper = mapper;
            _uow = uow;
        }
        /// <summary>
        /// Adds a new MCQ.
        /// </summary>
        /// <param name="mcqDto">The DTO containing the MCQ data.</param>
        /// <returns>A response indicating the result of the add operation.</returns>
        /// <response code="200">Returns a success message if the MCQ was added successfully.</response>
        /// <response code="400">Returns an error message if the MCQ already exists.</response>
        [HttpPost]
        [ProducesResponseType(typeof(APIResponse<string>), 200)]
        public async Task<IActionResult> Add([FromBody] McqDto mcqDto)
        {
            if (await _mcqRepository.IsMcqExist(mcqDto.Coursename, mcqDto.Question))
            {
                return BadRequest(new APIResponse<string>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = new List<string> { "MCQ already exists" }
                });
            }

            var mcq = _mapper.Map<McqDto, MCQ>(mcqDto);
            _mcqRepository.AddMCQ(mcq);
            await _uow.Complete();

            return Ok(new APIResponse<string>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = "MCQ added successfully"
            });
        }

        /// <summary>
        /// Adds a list of MCQs in bulk.
        /// </summary>
        /// <param name="mcqDtoList">The list of DTOs containing the MCQ data.</param>
        /// <returns>A response indicating the result of the bulk add operation.</returns>
        /// <response code="200">Returns a success message if the MCQs were added successfully, including any failures.</response>
        [HttpPost("bulk")]
        [ProducesResponseType(typeof(APIResponse<string>), 200)]
        public async Task<IActionResult> AddMcqList([FromBody] List<McqDto> mcqDtoList)
        {
            int failedToAdd = 0;

            foreach (var mcqDto in mcqDtoList)
            {
                if (await _mcqRepository.IsMcqExist(mcqDto.Coursename, mcqDto.Question))
                {
                    failedToAdd++;
                    continue;
                }

                var mcq = _mapper.Map<McqDto, MCQ>(mcqDto);
                _mcqRepository.AddMCQ(mcq);
            }

            await _uow.Complete();

            return Ok(new APIResponse<string>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = failedToAdd > 0 
                        ? $"{failedToAdd} MCQs already exist and were not added to the database"
                        : "All MCQs added successfully"
            });
        }

        /// <summary>
        /// Updates an existing MCQ.
        /// </summary>
        /// <param name="id">The ID of the MCQ to update.</param>
        /// <param name="mcqDto">The updated MCQ data.</param>
        /// <returns>A response indicating the result of the update operation.</returns>
        /// <response code="200">Returns a success message if the MCQ was updated successfully.</response>
        /// <response code="404">Returns an error message if mcq with ID does not exist.</response>

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(APIResponse<string>), 200)]
        public async Task<IActionResult> Update(int id, [FromBody] McqDto mcqDto)
        {
            var mcq =await _mcqRepository.GetMcqAsync(id);
            if (mcq == null)
            {
                return NotFound(new APIResponse<string>
                {
                    IsSuccess = false,
                    StatusCode= HttpStatusCode.NotFound,
                    ErrorMessages = new List<string> { $"MCQ with ID {id} not found."}
                });
            }

            var updatedMcq = _mapper.Map<McqDto, MCQ>(mcqDto, mcq);

            _mcqRepository.UpdateMcq(updatedMcq);
            await _uow.Complete();
            return Ok(new APIResponse<string>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = "Updated successfully"
            });
        }

        /// <summary>
        /// Deletes an existing MCQ by its ID.
        /// </summary>
        /// <param name="id">The ID of the MCQ to delete.</param>
        /// <returns>A response indicating the result of the delete operation.</returns>
        /// <response code="200">Returns a success message if the MCQ was deleted successfully.</response>
        /// <response code="404">Returns an error message if the MCQ does not exist.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(APIResponse<string>), 200)]
        public async Task<IActionResult> Delete(int id)
        {
            var mcq = await _mcqRepository.GetMcqAsync(id);
            if (mcq == null)
            {
                return NotFound(new APIResponse<string>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorMessages = new List<string> { $"MCQ with ID {id} not found." }
                });
            }

            await _mcqRepository.DeleteMcq(id);
            await _uow.Complete();

            return Ok(new APIResponse<string>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = "Deleted successfully."
            });
        }


        /// <summary>
        /// Deletes an option from an existing MCQ by the option's ID.
        /// </summary>
        /// <param name="mcqId">The ID of the MCQ.</param>
        /// <param name="optionId">The ID of the option to delete.</param>
        /// <returns>A response indicating the result of the delete operation.</returns>
        /// <response code="200">Returns a success message if the option was deleted successfully.</response>
        /// <response code="404">Returns an error message if the MCQ.</response>
        
        [HttpDelete("mcq/{mcqId}/option/{optionId}")]
        [ProducesResponseType(typeof(APIResponse<string>), 200)]
        public async Task<IActionResult> DeleteOption(int mcqId, int optionId)
        {
            var mcq = await _mcqRepository.GetMcqAsync(mcqId);
            if (mcq == null)
            {
                return NotFound(new APIResponse<string>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorMessages = new List<string> { $"MCQ with ID {mcqId} not found." }
                });
            }
            await _mcqRepository.DeleteMcqOption(mcqId, optionId);
            await _uow.Complete();

            return Ok(new APIResponse<string>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = $"Option {optionId} removed from MCQ {mcqId}."
            });
        }

    }
}
