using System.ComponentModel.DataAnnotations;


namespace api.DTOs
{
    public class AnswerCreateDto
    {
        public string Text{get; set; }=string.Empty;
        public bool IsCorrect{get; set; }
        public string OptionLetter{get; set; } = string.Empty; 
    }
}