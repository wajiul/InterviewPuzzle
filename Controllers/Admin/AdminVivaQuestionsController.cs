using AutoMapper;
using InterviewPuzzle.Data_Access.Repository;
using InterviewPuzzle.Data_Access;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InterviewPuzzle.Controllers.DTO;
using InterviewPuzzle.Data_Access.Model;
using InterviewPuzzle.Exceptions;

namespace InterviewPuzzle.Controllers.Admin
{
    [Route("api/admin/interviews")]
    [ApiController]
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
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] VivaQuestionDto questionDto)
        {
            var exist = await _questionRepository.isQuestionExist(questionDto.Coursename, questionDto.Text);
            if (exist)
                throw new AlreadyExistException("Question already exist");

            var question = _mapper.Map<VivaQuestionDto, VivaQuestion>(questionDto);

            _questionRepository.AddVavaQuestion(question);
            await _uow.Complete();
            return CreatedAtAction("Get", new { id = question.Id }, question);
        }
        [HttpPost("bulk")]
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

                _questionRepository.AddVavaQuestion(question);
            }
            await _uow.Complete();
            if (failedToAdd > 0)
                return Ok($"{failedToAdd} Viva questions already exist and not added to the database");

            return Ok("All viva questions added successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VivaQuestionDto questionDto)
        {
            var question = await _questionRepository.GetVivaQuestionAsync(id);
            if (question == null)
                throw new NotFoundException($"Viva question with Id {id} not found");
            var newQuestion = _mapper.Map<VivaQuestionDto, VivaQuestion>(questionDto, question);

            _questionRepository.UpdateVivaQuestion(newQuestion);
            await _uow.Complete();
            return Ok("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var question = await _questionRepository.GetVivaQuestionAsync(id);
            if (question == null)
                throw new NotFoundException($"Viva question with Id {id} not found");
            _questionRepository.DeleteVivaQuestion(question);
            await _uow.Complete();
            return Ok("Deleted Successfully");
        }
    }
}
