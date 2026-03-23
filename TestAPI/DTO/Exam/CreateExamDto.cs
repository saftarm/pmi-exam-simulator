namespace TestAPI.DTO
{
    public class CreateExamDto
    {

        public int CategoryId {get;set;}
        public string? Title { get; set; }

        public string? Context { get; set; }

        public int PassScore {get;set;}
        
        public int DurationInMinutes { get; set; }

        public int NumberOfQuestions { get; set; }

        public IEnumerable<CreateDomainDto>? CreateDomainDtos {get;set;}


    }
}
