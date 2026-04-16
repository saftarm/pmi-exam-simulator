using System.ComponentModel.DataAnnotations;

namespace TestAPI.DTO.Category
{
    public class AddExamsToCategoryDto
    {
        public Guid CategoryId { get; set; }
        public ICollection<Guid>? ExamIds { get; set; }
    }
}
