using System.ComponentModel.DataAnnotations;

namespace TestAPI.Entities
{
    public class Domain : BaseEntity
    {

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;


        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0, 99)]
        public int Weight { get; set; }

        public Guid ExamId { get; set; }

        public Exam? Exam { get; set; }

        public ICollection<Question> Questions { get; set; } = new List<Question>();

        public ICollection<DomainPerformance> DomainPerformances { get; set; } = new List<DomainPerformance>();

        public ICollection<UserExamResponse>? UserExamResponses {get;set;}




    }

}
