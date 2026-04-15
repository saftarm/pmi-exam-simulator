using System.ComponentModel.DataAnnotations;

namespace TestAPI.Entities
{
    public class UserExamResponse : BaseEntity
    {

        public bool IsCorrect { get; set; }

        public Guid QuestionId { get; set; }

        public Question? Question { get; set; }

        public Guid DomainId {get;set;}

        public Domain? Domain {get;set;}

        public Guid SelectedOptionId { get; set; }

        public AnswerOption? SelectedOption { get; set; }

        public Guid ExamAttemptId { get; set; }

        public ExamAttempt? ExamAttempt { get; set; }

    }
}
