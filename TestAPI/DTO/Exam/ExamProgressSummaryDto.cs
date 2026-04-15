namespace TestAPI.DTO
{
    public class ExamProgressSummaryDto
    {
        public Guid ExamId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int LatestScore { get; set; }
        public int AverageScore { get; set; }
        public int HighestScore { get; set; }
        public int TotalAttempts { get; set; }
        public DateTime LastAttemptDate { get; set; }
        public int TotalQuestionAnswered { get; set; }
        public int TotalCorrectAnswered { get; set; }
        public TimeSpan TimePerQuestion { get; set; }
        public int DurationInMinutes { get; set; }
    }
}

