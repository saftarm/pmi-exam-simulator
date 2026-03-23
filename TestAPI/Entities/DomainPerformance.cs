using System.ComponentModel.DataAnnotations;

namespace TestAPI.Entities
{
    public class DomainPerformance
    {
        [Key]
        public int Id { get; set; }

        public int UserId {get;set;}

        public int ExamId {get; set;}

        public Exam? Exam {get;set;}

        public int DomainId {get; set;}

        public Domain? Domain {get; set;}


        public int TotalAnswered {get;set;}

        public int TotalCorrect {get;set;}

        public DateTime LastUpdated {get;set;}
        


    }
}
