using System.ComponentModel.DataAnnotations;


namespace api.Models
{
    public class AnswerCreateDTo
    {
        public string Text{get; set; }=string.Empty;
        public bool IsCorrect{get; set; }
        public string OptionLetter{get; set; } = string.Empty; 
    }
}