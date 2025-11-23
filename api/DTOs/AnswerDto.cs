namespace api.DTOs
{
    public class AnswerDto
    {
        public int AnswerId { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }
}