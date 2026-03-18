using TestAPI.Entities;
using TestAPI.Models;

namespace TestAPI.DTO
{
    public class DeleteExamsRequest
    {

        public ICollection<int>? ExamIds { get; set; }

    }
}

