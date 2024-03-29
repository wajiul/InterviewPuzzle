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
    [Route("api/interviews")]
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
        [HttpGet("bycourse")]
        public async Task<IActionResult> GetQuestionsByCourse([FromQuery] string course)
        {
            var questions = await _questionRepository.GetQuestionByCourseAsync(course);
            if (questions == null || questions.Count == 0)
            {
                throw new NotFoundException($"Viva questions with course '{course}' not found");
            }
            return Ok(questions);
        }
    }
}
