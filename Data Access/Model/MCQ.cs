using System.ComponentModel.DataAnnotations;

namespace InterviewPuzzle.Data_Access.Model
{
    public class MCQ
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string CourseName { get; set; } = string.Empty;
        [Required]
        [StringLength(200)]
        public string Question { get; set; } = string.Empty;
        [Required]
        public List<Option> Options { get; set; }
        [Required]
        public int AnswerId { get; set; }
        [StringLength(200)]
        public string Note { get; set; } = string.Empty;
        public MCQ()
        {
            Options = new List<Option>();
        }

    }
}
