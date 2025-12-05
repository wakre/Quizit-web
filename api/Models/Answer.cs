using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    // this class represents a single answer option for a quiz question
    public class Answer
    {
        [Key]
        public int AnswerId { get; set; } //primary key of the anwer 

        [Required]
        [MaxLength(200)] // to avoid very long answers 
        public string Text { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }


        // Foreign key relationship
        [ForeignKey("Question")]
        public int QuestionId { get; set; } //

        public Question Question { get; set; } = null!;
    }
}