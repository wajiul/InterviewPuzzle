using System.ComponentModel.DataAnnotations;

namespace InterviewPuzzle.Data_Access.Model
{
    public class Login
    {
        [Required]
        [MinLength(3)]
        public string Username { get; set; } = string.Empty;
        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
        public bool Remember { get; set; }

    }
}
