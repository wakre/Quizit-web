using System.ComponentModel.DataAnnotations;

namespace api.DTOs
{
    public class UserRegisterDto
    {
        [Required]
        [MaxLength(40)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters.")]
        public string Password { get; set; } = string.Empty;
    }
}