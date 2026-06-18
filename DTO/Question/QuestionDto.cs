using System.Text.Json.Serialization;
using TestAPI.DTO.AnswerOption;

namespace TestAPI.DTO.Question
{
  public class QuestionDto
  {
    [JsonPropertyName("questionId")]
    public Guid Id { get; set; }

    [JsonPropertyName("questionTitle")]
    public string? Title { get; set; }

    [JsonPropertyName("answerOptions")]
    public IEnumerable<AnswerOptionDto>? AnswerOptionsDtos { get; set; }

  }
}

