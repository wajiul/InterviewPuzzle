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
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] McqDto mcqDto)
        {
            if (await _mcqRepository.IsMcqExist(mcqDto.Coursename, mcqDto.Question))
                throw new AlreadyExistException("MCQ already exist");

            var mcq = _mapper.Map<McqDto, MCQ>(mcqDto);

            _mcqRepository.AddMCQ(mcq);

            await _uow.Complete();
            return Ok("MCQ added successfully");
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> AddMcqList([FromBody] List<McqDto> mcqDtoList)
        {
            int failedtoAdd = 0;

            foreach (var mcqDto in mcqDtoList)
            {
                if (await _mcqRepository.IsMcqExist(mcqDto.Coursename, mcqDto.Question))
                {
                    failedtoAdd++;
                    continue;
                }

                var mcq = _mapper.Map<McqDto, MCQ>(mcqDto);

                _mcqRepository.AddMCQ(mcq);
            }
            await _uow.Complete();
            if (failedtoAdd > 0)
                return Ok($"{failedtoAdd} MCQ already exist and not added to the database");

            return Ok("All MCQ added successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MCQ mcq)
        {
            _mcqRepository.UpdateMcq(mcq);
            await _uow.Complete();
            return Ok("Data updated successfully");
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

    }
}
