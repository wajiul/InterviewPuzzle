using AutoMapper;
using InterviewPuzzle.Data_Access.Context;
using InterviewPuzzle.Data_Access.Repository;
using InterviewPuzzle.Data_Access;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InterviewPuzzle.Controllers.DTO;
using InterviewPuzzle.Exceptions;
using InterviewPuzzle.Data_Access.Model;
using AutoMapper.Configuration.Conventions;

namespace InterviewPuzzle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var questions =  await _questionRepository.GetCodingQuestionsAsync();
            return Ok(questions);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var question = await _questionRepository.GetCodingQuestionAsync(id);
            if (question == null)
            {
                throw new NotFoundException($"Coding question with Id {id} not found");
            }

            return Ok(question);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CodingQuestionDto questionDto)
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
