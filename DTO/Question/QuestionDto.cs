using System.Text.Json.Serialization;
using TestAPI.DTO.AnswerOption;
using TestAPI.Entities;

namespace TestAPI.DTO.Question
{
    public class QuestionDto
    {
        [JsonPropertyName("questionId")]
        public Guid Id { get; set; }

        [JsonPropertyName("questionTitle")]
        public string? Title { get; set; }

        [JsonPropertyName("questionType")]
        public QuestionType QuestionType { get; set; }

        [JsonPropertyName("answerOptions")]
        public IEnumerable<AnswerOptionDto>? AnswerOptionsDtos { get; set; }

    }
}

