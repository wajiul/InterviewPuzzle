using System.ComponentModel.DataAnnotations;

namespace InterviewPuzzle.Data_Access.Model
{
    public class Register
    {
        [Required]
        [MinLength(3)]
        public string Username { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MinLength (6)]
        public string Password { get; set; } = string.Empty;
        [Required]
        [Compare("Password", ErrorMessage="Password not matched")]
        [MinLength(6)]
        public string ConfirmPassword { get; set; } = string.Empty;

    }
}
