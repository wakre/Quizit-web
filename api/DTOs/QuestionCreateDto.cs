using System.ComponentModel.DataAnnotations;

namespace api.DTOs
{
    public class QuestionCreateDto
    {
        [Required]
        [MaxLength(500)]
        public string Text { get; set; } = string.Empty; //a required question text with limitation 

        [Required]
        public int QuizId { get; set; }

        [Required]
        [MaxLength(4, ErrorMessage = "A question can have maximum 4 options.")] //validation for max options a question can have  
        [MinLength(2, ErrorMessage = "A question must have at least 2 options.")] //validation for min options a question can have 
        public List<string> Options { get; set; } = new();  // List of answer options for the question

        [Required]
        [Range(0, 3, ErrorMessage = "Correct option index must be between 0 and 3.")] 
        public int CorrectOptionIndex { get; set; }
    }
}