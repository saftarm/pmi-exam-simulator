using TestAPI.Entities;
using TestAPI.Models;

namespace TestAPI.DTO
{
    public class DeleteQuestionsRequest
    {
        public ICollection<int>? QuestionIds { get; set; }

    }
}

