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
using Microsoft.AspNetCore.Authorization;

namespace InterviewPuzzle.Controllers
{
    [Route("api/coding")]
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

        [HttpGet("bytag")]
        public async Task<IActionResult> GetQuestionByTag([FromQuery] string tag)
        {
            var questions = await _questionRepository.GetCodingQuestionsByTagAsync(tag);
            if(questions == null || questions.Count == 0)
            {
                throw new NotFoundException($"Coding question with tag '{tag}' not found");
            }
            return Ok(questions);
        }

    }
}
