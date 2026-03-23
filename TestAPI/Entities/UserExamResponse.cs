using System.ComponentModel.DataAnnotations;

namespace TestAPI.Entities
{
    public class UserExamResponse
    {
        [Key]
        public int Id { get; set; }

        public bool IsCorrect { get; set; }

        public int QuestionId {get;set;}

        public Question? Question {get;set;}

        public int SelectedOptionId {get;set;}

        public AnswerOption? SelectedOption {get;set;}

        public int ExamAttemptId {get;set;}

        public ExamAttempt? ExamAttempt {get;set;}
        
    }
}
