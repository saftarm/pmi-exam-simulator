using TestAPI.Entities;
using TestAPI.Enums;

namespace TestAPI.DTO
{
    public class ExamProgressSummaryDto
    { 
        public int ExamId { get; set; }
        public string Title { get; set; } = string.Empty;

        public AttemptStatus Status {get;set;}

        public int HighestScore {get;set;}
        public int TotalAttempts {get;set;}

        public DateTime LastAttemptDate {get;set;}

        public int NumberOfQuestions { get; set; }

        public int DurationInMinutes { get; set; }

    }
}

