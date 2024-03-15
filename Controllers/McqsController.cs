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
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] McqDto mcqDto)
        {
            if (await _mcqRepository.IsMcqExist(mcqDto.CourseName, mcqDto.Question))
               throw new AlreadyExistException("Mcq already exist");

            var mcq = _mapper.Map<McqDto, MCQ>(mcqDto);

            _mcqRepository.AddMCQ(mcq);

            await _uow.Complete();
            return Ok("Mcq added");
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
            return Ok(mcq);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mcqRepository.DeleteMcq(id);
            await _uow.Complete();
            return Ok("Deleted successfully.");
        }

        [HttpDelete("mcq/{mcqId}/option/{optionId}")]
        public async Task<IActionResult> DeleteOption(int mcqId, int optionId)
        {
           await _mcqRepository.DeleteMcqOption(mcqId, optionId);
           await _uow.Complete();
            return Ok($"Option {optionId} Removed from Mcq {mcqId}");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MCQ mcq)
        {
            _mcqRepository.UpdateMcq(mcq);
            await _uow.Complete();
            return Ok("Data updated successfully");
        }

    }
}
