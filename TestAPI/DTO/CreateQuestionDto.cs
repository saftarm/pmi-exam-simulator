using TestAPI.Entities;
using TestAPI.Models;

namespace TestAPI.DTO
{
    public class CreateQuestionDto
    {
        public string? Text { get; set; }
        public ICollection<CreateAnswerOptionDto>? AnswerOptionsDtos { get; set; }

    }
}
