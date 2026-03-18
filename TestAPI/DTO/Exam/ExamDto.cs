namespace TestAPI.DTO
{
    public class ExamDto
    {

        public string? Title { get; set; }

        public int NumberOfQuestions { get; set; }

        public int DurationInMinutes { get; set; }

        public List<QuestionDto>? QuestionDtos { get; set; }

        public ICollection<CreateDomainDto> DomainDtos { get; set; }



    }
}

