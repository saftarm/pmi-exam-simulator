using System.ComponentModel.DataAnnotations;
using TestAPI.Enums;
namespace TestAPI.Entities
{
    public class ExamAttempt
    {
        [Key]
        public int Id {get; set;}
        public int UserId {get;set;}
        public int ExamId {get;set;}

        [Required]
        [MaxLength(100)]
        public string ExamTitle {get;set;} = string.Empty;

        public Exam? Exam {get;set;}

        public int CorrectCount {get; set;}

        [Range(0, 99)]
        public int Score { get;set;}


        public DateTime StartedAt {get;set;}
        
        public DateTime? SubmittedAt {get;set;}

        public AttemptStatus Status { get; set; }

        public ICollection<UserExamResponse> UserExamResponses {get;set;} = new List<UserExamResponse>();
        


    }
}
