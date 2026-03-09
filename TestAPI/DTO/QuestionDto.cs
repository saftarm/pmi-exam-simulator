using TestAPI.Entities;
using TestAPI.Models;

namespace TestAPI.DTO
{
    public class QuestionDto
    {


        public string? Text { get; set; }

        public ICollection<AnswerOptionDto>? AnswerOptionsDtos { get; set; }

    }
}
