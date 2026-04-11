using System.ComponentModel.DataAnnotations;

namespace TestAPI.Entities
{
    public class Question : BaseEntity
    {
        [Required]
        [MaxLength(1000)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string Explanation { get; set; } = string.Empty;

        public QuestionType QuestionType { get; set; }

        public Guid DomainId { get; set; }

        public Domain? Domain { get; set; } = null;

        public Guid? ExamId { get; set; }

        public Exam? Exam { get; set; } = null;
        public ICollection<AnswerOption> AnswerOptions { get; set; } = new List<AnswerOption>();

        public ICollection<UserExamResponse> UserExamResponses { get; set; } = new List<UserExamResponse>();




    }
}
