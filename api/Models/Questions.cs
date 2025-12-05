using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace api.Models
{
    // Represents a question within a quiz, with answer options 
    public class Question : IValidatableObject 
    {
        [Key]
        public int QuestionId { get; set; } // Primary key for question

        [Required(ErrorMessage = "Please add a question")]
        [MaxLength(500)]
        public string Text { get; set; } = string.Empty; // Question text

        public string? ImageUrl { get; set; } // Optional URL for image connected to question

        
        public List<Answer> Answers { get; set; } = new(); // list of Answer options connected to the question

        [ForeignKey("Quiz")]
        public int QuizId { get; set; } // Foreign key pointing to the quiz

        public Quiz Quiz { get; set; } = null!; // Navigation: property to the quiz 

        // Validation logic to enforce correct answer constraint 
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Answers == null || Answers.Count < 2) //ensure that a questions have min 2 options
            {
                yield return new ValidationResult("The question must have at least 2 answers.",
                    new[] { nameof(Answers) });
            }
            else if (Answers.Count > 4)
            {
                yield return new ValidationResult("The question has a maximum of 4 answers.", //ensure that a question  have max 4 option 
                    new[] { nameof(Answers) });
            }

            if (!Answers!.Any(a => a.IsCorrect)) //ensure that at least one answer is markes as correct 
            {
                yield return new ValidationResult("Each question must have at least one correct answer.",
                    new[] { nameof(Answers) });
            }
        }
    }
}