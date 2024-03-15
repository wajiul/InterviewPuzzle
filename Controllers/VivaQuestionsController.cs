using AutoMapper;
using InterviewPuzzle.Data_Access.Context;
using InterviewPuzzle.Data_Access.Repository;
using InterviewPuzzle.Data_Access;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InterviewPuzzle.Exceptions;
using InterviewPuzzle.Data_Access.Model;
using InterviewPuzzle.Controllers.DTO;

namespace InterviewPuzzle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VivaQuestionsController : ControllerBase
    {
        private readonly InterviewPuzzleDbContext _context;
        private readonly VivaQuestionRepository _questionRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public VivaQuestionsController(InterviewPuzzleDbContext context, VivaQuestionRepository questionRepository, IMapper mapper, IUnitOfWork uow)
        {
            _context = context;
            _questionRepository = questionRepository;
            _mapper = mapper;
            _uow = uow;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var questions = await _questionRepository.GetVivaQuestionsAsync();
            return Ok(questions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var question = await _questionRepository.GetVivaQuestionAsync(id);
            if(question == null)
            {
                throw new NotFoundException($"Viva question with Id {id} not found");
            }
            return Ok(question);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] VivaQuestionDto questionDto)
        {
            var exist = await _questionRepository.isQuestionExist(questionDto.CourseName, questionDto.Text);
            if (exist)
                throw new AlreadyExistException("Question already exist");

            var question = _mapper.Map<VivaQuestionDto, VivaQuestion>(questionDto);

            _questionRepository.AddVavaQuestion(question);
            await _uow.Complete();
            return CreatedAtAction("Get", new { id = question.Id }, question);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VivaQuestionDto questionDto)
        {
            var question = await _questionRepository.GetVivaQuestionAsync(id);
            if (question == null)
                throw new NotFoundException($"Viva question with Id {id} not found.");
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
                throw new NotFoundException($"Viva question with Id {id} not found.");
            _questionRepository.DeleteVivaQuestion(question);
            await _uow.Complete();
            return Ok("Deleted Successfully."); 
        }

    }
}
