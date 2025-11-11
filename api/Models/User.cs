using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class User
    {
        public int UserId { get; set; }

        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ .\-]{2,40}", ErrorMessage = "The name must be numbers or letters and between 2 to 40 characters.")]
        [Display(Name = "User Name")]
        public string UserName { get; set; } = string.Empty;
        [Required]

        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ .\-]{9,40}", ErrorMessage = " you need a strong password with minimum 9 characters ")]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;
        [Required]


        public required List<Quiz> Quizzes { get; set;  }

    }
}