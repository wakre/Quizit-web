namespace api.DTOs
{
    public class QuizDto
    {
        public int QuizId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; } //option description of the quiz 
        public string? ImageUrl { get; set; } //option to upload an imag to the quuiz 
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public List<QuestionDto> Questions { get; set; } = new();
    }
}