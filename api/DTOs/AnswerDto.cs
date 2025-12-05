namespace api.DTOs
{
    public class AnswerDto
    {
        public int AnswerId { get; set; } // Unique identifier for the answer
        public string Text { get; set; } = string.Empty; // The answer text for the option
        public bool IsCorrect { get; set; } // Indicates whether this option is correct
    }
}