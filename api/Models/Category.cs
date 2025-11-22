using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; } // Unique ID for the category

        [Required(ErrorMessage = "Category name is required")]
        [MaxLength(100)] // length limit
        public string Name { get; set; } = string.Empty; // Category name

        public List<Quiz> Quizzes { get; set; } = new();
    }
}