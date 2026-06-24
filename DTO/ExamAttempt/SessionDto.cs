using TestAPI.DTO.Question;

namespace TestAPI.DTO.ExamAttempt
{

    public record SessionDto
    {
        public Guid SessionId { get; set; }
        public IEnumerable<QuestionDto> Questions { get; set; } = [];

    }
}
