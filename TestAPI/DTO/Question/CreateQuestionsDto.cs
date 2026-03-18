using TestAPI.Entities;
using TestAPI.Models;

namespace TestAPI.DTO
{
    public class CreateQuestionsDto
    {

        public ICollection<CreateQuestionDto>? CreateQuestionDtos { get; set; }

    }
}

