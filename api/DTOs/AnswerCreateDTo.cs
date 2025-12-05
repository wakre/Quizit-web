using System.ComponentModel.DataAnnotations;

namespace api.DTOs
{
    public class AnswerCreateDto
    {
        [Required]
        [MaxLength(200)]
        public string Text { get; set; } = string.Empty; // a required answer text for the option with limition 

        public bool IsCorrect { get; set; } // Indicates whether this option is corr
    }
}