namespace TestAPI.DTO
{
    public class CreateExamDto
    {

        public int CategoryId {get;set;}
        public string Title { get; set; } = string.Empty;

        public string Context { get; set; } = string.Empty;

        public int PassScore {get;set;}
        
        public int DurationInMinutes { get; set; }

        public int NumberOfQuestions { get; set; }

        public IEnumerable<CreateDomainDto> CreateDomainDtos {get;set;} = new List<CreateDomainDto>();


    }
}
