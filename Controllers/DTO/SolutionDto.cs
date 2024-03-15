using System.ComponentModel.DataAnnotations;

namespace InterviewPuzzle.Controllers.DTO
{
    public class SolutionDto
    {
        [Required]
        [StringLength(30)]
        public string Language { get; set; } = string.Empty;
        [Required]
        public string Code { get; set; } = string.Empty;
    }
}
