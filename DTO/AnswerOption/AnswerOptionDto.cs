namespace TestAPI.DTO.AnswerOption
{
    public class AnswerOptionDto
    {
        public Guid Id {get;set;}
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }
}

