using System.ComponentModel.DataAnnotations;

namespace InterviewPuzzle.Controllers.DTO
{
    public class VivaQuestionDto
    {
        [Required]
        [StringLength(50)]
        public string Coursename { get; set; } = string.Empty;
        [Required]
        [StringLength(200)]
        public string Text { get; set; } = string.Empty;
        [Required]
        public string Answer { get; set; } = string.Empty;
    }
}
