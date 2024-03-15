using System.ComponentModel.DataAnnotations;

namespace InterviewPuzzle.Data_Access.Model
{
    public class CodingQuestion
    {
        public CodingQuestion()
        {
            Solutions = new List<Solution>();
        }
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Text { get; set; } = string.Empty;
        [Required]
        public List<Solution> Solutions { get; set; }
        
    }
}
