using AutoMapper;
using InterviewPuzzle.Controllers.DTO;
using InterviewPuzzle.Data_Access;
using InterviewPuzzle.Data_Access.Model;
using InterviewPuzzle.Data_Access.Repository;
using InterviewPuzzle.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InterviewPuzzle.Controllers.Admin
{
    [Route("api/admin/coding")]
    [ApiController]
    [Authorize(Roles = "admin")]
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


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CodingQuestionDto questionDto)
        {
            var exist = await _questionRepository.IsQuestionExist(questionDto.Text);
            if (exist)
                throw new AlreadyExistException("Coding question already exist");

            var question = _mapper.Map<CodingQuestionDto, CodingQuestion>(questionDto);

            _questionRepository.Add(question);
            await _uow.Complete();
            return CreatedAtAction("Get", new { id = question.Id }, question);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CodingQuestionDto questionDto)
        {
            var question = await _questionRepository.GetCodingQuestionAsync(id);
            if (question == null)
                throw new NotFoundException($"Coding question with Id {id} not found.");
            var newQuestion = _mapper.Map<CodingQuestionDto, CodingQuestion>(questionDto, question);

            _questionRepository.UpdateCodingQuestion(newQuestion);
            await _uow.Complete();
            return Ok("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var question = await _questionRepository.GetCodingQuestionAsync(id);
            if (question == null)
                throw new NotFoundException($"Coding question with Id {id} not found.");
            _questionRepository.DeleteCodingQuestion(question);
            await _uow.Complete();
            return Ok("Deleted Successfully.");
        }

        [HttpDelete("codingQuestions/{questionId}/solution/{solutionId}")]
        public async Task<IActionResult> DeleteOption(int questionId, int solutionId)
        {
            await _questionRepository.DeleteSolution(questionId, solutionId);
            await _uow.Complete();
            return Ok($"Solution Id {solutionId} Removed from  Question {questionId}");
        }

    }
}
