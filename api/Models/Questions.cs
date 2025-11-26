using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace api.Models
{
    public class Question : IValidatableObject // Validation logic
    {
        [Key]
        public int QuestionId { get; set; } // Primary key for question

        [Required(ErrorMessage = "Please add a question")]
        [MaxLength(500)]
        public string Text { get; set; } = string.Empty; // Question text

        public string? ImageUrl { get; set; } // Optional URL for image connected to question

        // Answers connected to the question
        public List<Answer> Answers { get; set; } = new();

        [ForeignKey("Quiz")]
        public int QuizId { get; set; } // Foreign key pointing to the quiz

        public Quiz Quiz { get; set; } = null!; // Navigation property, quiz object

        // Validation logic for question class
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Various validation errors
            if (Answers == null || Answers.Count < 2)
            {
                yield return new ValidationResult("The question must have at least 2 answers.",
                    new[] { nameof(Answers) });
            }
            else if (Answers.Count > 4)
            {
                yield return new ValidationResult("The question has a maximum of 4 answers.",
                    new[] { nameof(Answers) });
            }

            if (!Answers.Any(a => a.IsCorrect)) //sjekk det ???????
            {
                yield return new ValidationResult("Each question must have at least one correct answer.",
                    new[] { nameof(Answers) });
            }
        }
    }
}