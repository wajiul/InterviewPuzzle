using InterviewPuzzle.Data_Access.Model;
using System.ComponentModel.DataAnnotations;

namespace InterviewPuzzle.Controllers.DTO
{
    public class CodingQuestionDto
    {
        public CodingQuestionDto()
        {
            Solutions = new List<SolutionDto>();
        }
        [Required]
        [StringLength(200)]
        public string Text { get; set; } = string.Empty;
        [Required]
        public List<SolutionDto> Solutions { get; set; }
    }
}
