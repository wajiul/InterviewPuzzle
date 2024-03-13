using InterviewPuzzle.Data_Access.Context;
using InterviewPuzzle.Data_Access.Model;
using InterviewPuzzle.Data_Access.Repository;
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

        public McqsController(InterviewPuzzleDbContext context, McqRepository mcqRepository)
        {
            _context = context;
            _mcqRepository = mcqRepository;
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] MCQ mcq)
        {
            _mcqRepository.AddMCQ(mcq);
            await _context.SaveChangesAsync();
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
            _mcqRepository.DeleteMcq(id);
            await _context.SaveChangesAsync();
            return Ok("Deleted successfully.");
        }
        [HttpDelete("mcq/{mcqId}/option/{optionId}")]
        public async Task<IActionResult> DeleteOption(int mcqId, int optionId)
        {
            var optionToRemove = _context.options.Find(optionId);
            if(optionToRemove != null)
            {
                _context.options.Remove(optionToRemove);
            }
            await _context.SaveChangesAsync();
            return Ok($"Option {optionId} Removed from Mcq {mcqId}");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MCQ mcq)
        {
            _mcqRepository.UpdateMcq(mcq);
            await _context.SaveChangesAsync();
            return Ok("Data updated successfully");
        }

    }
}
