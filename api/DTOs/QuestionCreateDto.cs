using System.ComponentModel.DataAnnotations;

namespace api.DTOs
{
    public class QuestionCreateDto
    {
        [Required]
        [MaxLength(500)]
        public string Text { get; set; } = string.Empty;

        [Required]
        public int QuizId { get; set; }

        [Required]
        [MaxLength(4, ErrorMessage = "A question can have maximum 4 options.")]
        [MinLength(2, ErrorMessage = "A question must have at least 2 options.")]
        public List<string> Options { get; set; } = new();

        [Required]
        [Range(0, 3, ErrorMessage = "Correct option index must be between 0 and 3.")]
        public int CorrectOptionIndex { get; set; }
    }
}