using System.ComponentModel.DataAnnotations;

namespace api.DTOs
{
    public class AnswerCreateDto
    {
        [Required]
        [MaxLength(200)]
        public string Text { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }
    }
}