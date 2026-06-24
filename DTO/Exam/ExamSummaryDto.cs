using TestAPI.Enums;

namespace TestAPI.DTO
{
    public class ExamSummaryDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string CategoryTitle { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        public int NumberOfQuestions { get; set; }
        public int DurationInMinutes { get; set; }
        public ExamStatus Status { get; set; }
    }
}

