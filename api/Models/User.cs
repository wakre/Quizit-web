using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 2, ErrorMessage = "Username must be 2-40 characters.")]
        [RegularExpression(@"^[0-9a-zA-ZæøåÆØÅ .\-]+$", ErrorMessage = "Username can only contain letters and numbers")]
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
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "Password must contain at least one uppercase letter, and one digit.")]
        [Display(Name = "Password (input only)")]
        public string? Password { get; set; }

        public List<Quiz> Quizzes { get; set; } = new();
    }
}
