using TestAPI.Enums;
namespace TestAPI.Entities
{
    public class CreateExamAttemptDto
    {
        public Guid UserId {get;set;}
        public Guid ExamId {get;set;}
        public string? ExamTitle {get;set;}
        public User? User {get;set;}
        public int Score { get;set;}
        public DateTime StartedAt {get;set;}
        
        public DateTime? SubmittedAt {get;set;}

        public ExamStatus Status { get; set; }

        public ICollection<UserExamResponse>? UserExamResponses {get;set;} = new List<UserExamResponse>();
        


    }
}
