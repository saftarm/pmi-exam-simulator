namespace TestAPI.DTO
{
    public class QuestionDto
    {


        public int Id {get;set;}
        public string? Title { get; set; }

        public List<AnswerOptionDto>? AnswerOptionsDtos { get; set; }

    }
}

