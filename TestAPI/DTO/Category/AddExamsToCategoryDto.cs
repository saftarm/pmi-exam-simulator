using System.ComponentModel.DataAnnotations;

namespace TestAPI.DTO.Category
{
    public class AddExamsToCategoryDto
    {
       
        public int CategoryId { get; set; }
        public ICollection<int>? ExamIds { get; set; }
    }
}
