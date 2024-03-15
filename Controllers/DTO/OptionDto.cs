using System.ComponentModel.DataAnnotations;

namespace InterviewPuzzle.Controllers.DTO
{
    public class OptionDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
    }
}
