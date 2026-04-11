namespace TestAPI.DTO
{
    public class ExamFullDto
    { 
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public string CategoryTitle {get; set;} = string.Empty;

        public Guid CategoryId { get; set; }

        public int NumberOfQuestions { get; set; }

        public int DurationInMinutes { get; set; }

        public IEnumerable<QuestionDto>? QuestionDtos {get;set;}


    }
}

