using TestAPI.DTO.AnswerOption;
using TestAPI.Entities;

namespace TestAPI.DTO.Question
{
    public class QuestionAdminDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Explanation { get; set; }
        public QuestionType QuestionType { get; set; }
        public Guid DomainId { get; set; }
        public string DomainTitle { get; set; } = string.Empty;
        public string ExamTitle { get; set; } = string.Empty;
        public IEnumerable<AnswerOptionDto>? AnswerOptions { get; set; }
    }
}
