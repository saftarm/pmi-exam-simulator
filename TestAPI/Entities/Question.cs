using TestAPI.Models;

namespace TestAPI.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public string? Text { get; set; }

        public string? Explanation {get; set;}

        public int DomainId {get; set;}

        public Domain? Domain {get;set;}
        public List<AnswerOption>? AnswerOptions { get; set; }

        

        public ICollection<Exam>? Exams {get;set;}


    }
}
