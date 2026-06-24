using TestAPI.Entities;

namespace TestAPI.DTO.Question
{
    public class CreateQuestionDto
    {
        public string? Title { get; set; }
        public string? Explanation { get; set; }
        public QuestionType QuestionType { get; set; }
        public Guid DomainId { get; set; }
        public ICollection<CreateAnswerOptionDto> AnswerOptionsDtos { get; set; } = [];
    }
}

