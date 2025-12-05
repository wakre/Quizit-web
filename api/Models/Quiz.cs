using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    // Represents a quiz created by a user, containing questions and belonging to a category
    public class Quiz : IValidatableObject
    {
        [Key]
        public int QuizId { get; set; } //primary key of the quiz 

        [Required(ErrorMessage = "Title is required")]  
        [MaxLength(100)] 
        public string Title { get; set; } = string.Empty; //title of the quiz

        [MaxLength(500)] 
        public string? Description { get; set; } // Optional description of the quiz

        [Url] 
        public string? ImageUrl { get; set; } //optional Image URL for the quiz 

        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; } //foreign key linking to the category 

        [ForeignKey("CategoryId")]
        public Category Category { get; set; } = null!; // Navigation property to the category 

        public DateTime DateCreated { get; set; } = DateTime.UtcNow; //when the quiz was created 

        public int UserId { get; set; } // foregein key linkin to the quiz owner  

        [ForeignKey("UserId")]
        public User User { get; set; } = null!; // navigation property to the quiz owner

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