namespace TestAPI.DTO
{
    public class CreateExamDto
    {
        public string? Title { get; set; }
        public int DurationInMinutes { get; set; }

        public int NumberOfQuestions { get; set; }

        public IEnumerable<int>? QuestionIds { get; set; }

    }
}
