

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TestAPI.Entities
{
    public class Category : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string? Description { get; set; }

        public ICollection<Exam> Exams { get; set; } = new List<Exam>();


        [DefaultValue(0)]
        public int NumberOfExams { get; set; } 
    }
}