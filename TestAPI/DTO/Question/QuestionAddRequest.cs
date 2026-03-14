namespace TestAPI.DTO
{
    public class QuestionAddRequest
    {

        public string? Text { get; set; }
        public int? DifficultyLevel { get; set; }
        public int? AnswerType { get; set; }

        public List<AnswerOptionDto>? AnswerOptions { get; set; }

    }
}

