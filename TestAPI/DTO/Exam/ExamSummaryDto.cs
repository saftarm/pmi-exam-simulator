using System.ComponentModel.DataAnnotations;

namespace TestAPI.DTO
{
    public class ExamSummaryDto
    {
        public Guid Id { get; set; }
        [Required]
        public string? Title { get; set; } = string.Empty;

        [Required]
        public string CategoryTitle { get; set; } = string.Empty;

        public Guid CategoryId { get; set; }
        [Required]

        public int NumberOfQuestions { get; set; }

        [Required]
        public int DurationInMinutes { get; set; }
    }
}

