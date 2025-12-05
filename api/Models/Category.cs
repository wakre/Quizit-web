using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    // Represents a category to which quizzes can belong
    public class Category
    {
        [Key]
        public int CategoryId { get; set; } // primary key for the category

        [Required(ErrorMessage = "Category name is required")]
        [MaxLength(100)] // length limit
        public string Name { get; set; } = string.Empty; // Category name

        public List<Quiz> Quizzes { get; set; } = new(); // navigation: quizzes belonging to this category
    }
}