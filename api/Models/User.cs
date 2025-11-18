using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ .\-]{2,40}",
            ErrorMessage = "The name must be a numbers or letters between 2 to 40 characters.")]
        [Display(Name = "User Name")]
        public string UserName { get; set; } = string.Empty;


        [Required]
        [EmailAddress(ErrorMessage = "You must have a valid email")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;
        

        [Required]
        [Display(Name = "Password Hash")]
        public string PasswordHash { get; set; } = string.Empty;

        [NotMapped]
        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ .\-]{9,40}",
            ErrorMessage = "You need a strong password with minimum 9 characters!")]
        [Display(Name = "Password (input only)")]
        public string? Password { get; set; }
        
        public List<Quiz> Quizzes { get; set; } = new();

    }
}