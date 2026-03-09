namespace TestAPI.Entities
{
    public class UserExamResponse
    {

        public int Id { get; set; }

        public bool IsCorrect { get; set; }

        public int QuestionId {get;set;}

        public int SelectedOptionId {get;set;}

        public int ExamAttemptId {get;set;}

        public ExamAttempt? ExamAttempt {get;set;}
        
    }
}
