using AutoMapper;
using InterviewPuzzle.Controllers.DTO;
using InterviewPuzzle.Data_Access;
using InterviewPuzzle.Data_Access.Context;
using InterviewPuzzle.Data_Access.Model;
using InterviewPuzzle.Data_Access.Repository;
using InterviewPuzzle.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InterviewPuzzle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class McqsController : ControllerBase
    {
        private readonly InterviewPuzzleDbContext _context;
        private readonly McqRepository _mcqRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public McqsController(InterviewPuzzleDbContext context, McqRepository mcqRepository, IMapper mapper, IUnitOfWork uow)
        {
            _context = context;
            _mcqRepository = mcqRepository;
            _mapper = mapper;
            _uow = uow;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var mcqs = await _mcqRepository.GetAllMcqAsync();
            return Ok(mcqs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var mcq = await _mcqRepository.GetMcqAsync(id);
            if (mcq == null)
            {
                throw new NotFoundException($"MCQ with Id {id} not found");
            }
            return Ok(mcq);
        }
        [HttpGet("bycourse")]
        public async Task<IActionResult> GetMcqsByCourse([FromQuery] string course)
        {
            var mcqs = await _mcqRepository.GetMcqByCourseAsync(course.ToLower());
            if (mcqs == null || mcqs.Count == 0)
                throw new NotFoundException($"MCQ with course '{course}' not found");
            return Ok(mcqs);
        }
      
    }
}
