namespace TestAPI.Entities
{
    public class AnswerOption
    {

        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int ExamId {get;set;}

        public Question? Question {get;set;}

        public Exam? Exam {get;set;}
        
        public string? Text { get; set; }
        public bool IsCorrect { get; set; }
        
    }
}
