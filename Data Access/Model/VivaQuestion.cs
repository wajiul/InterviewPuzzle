using System.ComponentModel.DataAnnotations;

namespace InterviewPuzzle.Data_Access.Model
{
    public class VivaQuestion
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string CourseName { get; set; } = string.Empty;
        [Required]
        [StringLength(200)]
        public string Text { get; set; } = string.Empty;
        [Required]
        public string Answer { get; set; } = string.Empty;
    }
}
