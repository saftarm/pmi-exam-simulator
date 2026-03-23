using TestAPI.Entities;
using TestAPI.Models;

namespace TestAPI.DTO
{
    public class CreateQuestionDto
    {


        public string Title { get; set; } = string.Empty;

        public string Explanation {get;set;}= string.Empty;

        public int DomainId {get;set;}
        
        public ICollection<CreateAnswerOptionDto>? AnswerOptionsDtos { get; set; }

    }
}

