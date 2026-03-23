namespace TestAPI.DTO
{
    public class ExamFullDto
    { 
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public string CategoryTitle {get; set;} = string.Empty;

        public int CategoryId { get; set; }

        public int NumberOfQuestions { get; set; }

        public int DurationInMinutes { get; set; }

        public IEnumerable<QuestionDto>? QuestionDtos {get;set;}


    }
}

