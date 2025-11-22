using System.ComponentModel.DataAnnotations;


namespace api.DTOs
{
    public class QuizDto
    {
        public int QuizId {get; set; }
        public string Title {get; set; }= string.Empty; 
        public string? Description {get; set; }
        public string? ImageUrl {get; set; }
        public int CategoryId {get; set; }
        public string CategoryName {get; set; }= string.Empty; 
        public int UserId {get; set; }
        public string UserName {get; set; }= string.Empty; 
        public List<QuestionDto> Questions {get; set; } = new();

    }
}