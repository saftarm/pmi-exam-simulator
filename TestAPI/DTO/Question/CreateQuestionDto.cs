using TestAPI.Entities;
using TestAPI.Models;

namespace TestAPI.DTO
{
    public class CreateQuestionDto
    {


        public string? Title { get; set; }

        public string? Explanation {get;set;}

        public int DomainId {get;set;}
        
        public ICollection<CreateAnswerOptionDto>? AnswerOptionsDtos { get; set; }

    }
}

