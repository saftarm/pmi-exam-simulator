using TestAPI.Enums;
namespace TestAPI.Entities
{
    public class AttemptDetailDto
    {
        public Guid AttemptId {get;set;}
        public DateTime StartedAt {get;set;}
        public DateTime? CompletedAt { get; set; }
        public double? Score { get; set; }
        public int TotalQuestions { get; set; }
        public int? CorrectAnswers { get; set; }
        public int? TimeTakenMinutes { get; set; }
        public AttemptStatus Status { get; set; }

    }
}
