using TestAPI.Entities;
using TestAPI.Models;

namespace TestAPI.DTO
{
    public class DeleteQuestionsRequest
    {
        public ICollection<Guid>? QuestionIds { get; set; }

    }
}

