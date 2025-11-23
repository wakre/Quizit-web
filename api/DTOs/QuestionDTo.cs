namespace api.DTOs
{
    public class QuestionDto
    {
        public int QuestionId { get; set; }
        public string Text { get; set; } = string.Empty;
        public List<AnswerDto> Answers { get; set; } = new();
    }
}