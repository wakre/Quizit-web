using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    // Represents a user of the application who can create quizzes and manage their own questions
    public class User
    {
        [Key]
        public int UserId { get; set; } //primary key of the user 

        [Required]
        [StringLength(40, MinimumLength = 2, ErrorMessage = "Username must be 2-40 characters.")]
        [RegularExpression(@"^[0-9a-zA-ZæøåÆØÅ .\-]+$", ErrorMessage = "Username can only contain letters and numbers")]
        [Display(Name = "User Name")]
        public string UserName { get; set; } = string.Empty; // display name of the user

        [Required]
        [EmailAddress(ErrorMessage = "You must have a valid email")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty; //email for login 

        [Required]
        [Display(Name = "Password Hash")]
        public string PasswordHash { get; set; } = string.Empty; //hashed password for security ressons

        [NotMapped]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "Password must contain at least one uppercase letter, and one digit.")]
        [Display(Name = "Password (input only)")]
        public string? Password { get; set; } //plain text password input only and it not stored in databse 

        public List<Quiz> Quizzes { get; set; } = new(); // navigation: all quizzes created by this user
    }
}
