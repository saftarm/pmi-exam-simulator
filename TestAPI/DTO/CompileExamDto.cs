namespace TestAPI.DTO
{
    public class CompileExamDto
    {

        public string? Title {get;set;}
        
        public int DurationInMinutes {get;set;}

        public List<int>? QuestionIds {get;set; }

    }
}

