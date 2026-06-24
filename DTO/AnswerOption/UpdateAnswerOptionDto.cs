namespace TestAPI.DTO
{
    public class UpdateAnswerOptionDto
    {
        public Guid? Id { get; set; }
        public string Text { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }
    }

}

