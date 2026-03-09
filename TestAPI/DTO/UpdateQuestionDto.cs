using TestAPI.Entities;
using TestAPI.Models;

namespace TestAPI.DTO
{
    public class UpdateQuestionDto
    {
        public string? Text { get; set; }

        public ICollection<UpdateAnswerOptionDto>? AnswerOptionsDtos { get; set; }

    }
}
