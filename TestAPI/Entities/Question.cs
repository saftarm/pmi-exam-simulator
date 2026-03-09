using TestAPI.Models;

namespace TestAPI.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public string? Text { get; set; }

        public ICollection<AnswerOption>? AnswerOptions { get; set; }

        public ICollection<Exam>? Exams {get;set;}

        public ICollection<ExamQuestion>? ExamsQuestions { get; set; }


    }
}
