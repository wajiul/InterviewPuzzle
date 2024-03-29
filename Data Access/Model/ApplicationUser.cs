using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace InterviewPuzzle.Data_Access.Model
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        public string FirstName { get; set;} = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
    }
}
