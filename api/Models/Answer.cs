using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    // this class represents a single answer option for a quiz question
    public class Answer
    {
        [Key]
        public int AnswerId { get; set; }

        [Required]
        [MaxLength(200)] // to avoid very long answers 
        public string Text { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }

        // Optional: A letter label (A, B, C, D) - can be auto-generated in controller if needed
        // public string OptionLetter { get; set; } = string.Empty;

        // Foreign key relationship
        [ForeignKey("Question")]
        public int QuestionId { get; set; }

        public Question Question { get; set; } = null!;
    }
}