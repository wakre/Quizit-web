using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    // Represents a single answer option for a quiz question
    public class Answer
    {
        [Key]
        public int AnswerId { get; set; }

        [Required]
        public string Text { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }

        // Optional: a letter label (A, B, C, D)
        public string OptionLetter { get; set; } = string.Empty;

        // Foreign key relationship
        [ForeignKey("Question")]
        public int QuestionId { get; set; }

        public Question Question { get; set; } = null!;
    }
}
