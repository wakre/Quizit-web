using System.ComponentModel.DataAnnotations;

namespace api.DTOs
{
    public class QuizUpdateDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty; 

        [MaxLength(500)]
        public string? Description { get; set; }

        [Url]
        public string? ImageUrl { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}