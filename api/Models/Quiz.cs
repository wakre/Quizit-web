
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace api.Models
{
    public class Quiz
    {
        [Key]
        public int QuizId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; } //optional description of the text
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; } = null!; //navigation property

        public DateTime DateCreated { get; set; } = DateTime.Now;

        //Relation to the user who created/owned the quiz
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
        public List<Question> Questions { get; set; } = new(); //list of questions in quiz
    }
}