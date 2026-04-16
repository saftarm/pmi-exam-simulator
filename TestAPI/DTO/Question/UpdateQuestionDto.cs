using TestAPI.Entities;
using TestAPI.Models;

namespace TestAPI.DTO
{
    public class UpdateQuestionRequest
    {

        public Guid Id {get;set;}
        public string? Title { get; set; }

        public ICollection<UpdateAnswerOptionDto> AnswerOptionsDtos { get; set; } = new List<UpdateAnswerOptionDto>();

    }
}

