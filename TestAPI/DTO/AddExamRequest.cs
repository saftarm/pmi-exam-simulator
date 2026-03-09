namespace TestAPI.DTO
{
    public class AddExamRequest
    {
        public string? Title { get; set; }

        public int NumberOfQuestions { get; set; }

        public TimeSpan Duration { get; set; }

        public List<int>? QuestionIds { get; set; }

    }
}
