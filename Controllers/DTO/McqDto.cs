using System.ComponentModel.DataAnnotations;

namespace InterviewPuzzle.Controllers.DTO
{
    public class McqDto
    {

        [Required]
        [StringLength(50)]
        public string CourseName { get; set; } = string.Empty;
        [Required]
        [StringLength(200)]
        public string Question { get; set; } = string.Empty;
        [Required]
        public List<OptionDto> Options { get; set; }
        [Required]
        public int AnswerId { get; set; }
        [StringLength(200)]
        public string Note { get; set; } = string.Empty;
        public McqDto()
        {
            Options = new List<OptionDto>();
        }

    }
}
