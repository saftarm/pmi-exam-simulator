namespace TestAPI.DTO
{
    public class CreateExamDto
    {

        public string? Title { get; set; }

        public string? Context { get; set; }

        public int PassScore {get;set;}
        
        public int DurationInMinutes { get; set; }

        public int NumberOfQuestions { get; set; }

        public IEnumerable<CreateDomainDto>? createDomainDtos {get;set;}


    }
}
