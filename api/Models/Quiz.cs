using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Quiz : IValidatableObject // validation for max questions
    {
        [Key]
        public int QuizId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(100)] // Matches DTO
        public string Title { get; set; } = string.Empty;

        [MaxLength(500)] // Matches DTO
        public string? Description { get; set; } // Optional description of the quiz

        [Url] // Ensure valid URL
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; } = null!; // Navigation property

        public DateTime DateCreated { get; set; } = DateTime.UtcNow; 

        // Relation to the user who created/owned the quiz
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        public List<Question> Questions { get; set; } = new(); // List of questions in quiz

        // Validation for max 10 questions pr quiz
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Questions != null && Questions.Count > 10)
            {
                yield return new ValidationResult("A quiz can have maximum 10 questions.",
                    new[] { nameof(Questions) });
            }
        }
    }
}