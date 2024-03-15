using System.ComponentModel.DataAnnotations;

namespace InterviewPuzzle.Data_Access.Model
{
    public class Solution
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        public string Language { get; set; } = string.Empty;
        [Required]
        public string Code { get; set; } = string.Empty;
   
       
    }
}
