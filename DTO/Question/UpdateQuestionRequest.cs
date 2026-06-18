using TestAPI.Entities;

namespace TestAPI.DTO.Question
{
  public class UpdateQuestionRequest
  {
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Explanation {get;set;}
    public QuestionType QuestionType {get;set;}
    public ICollection<UpdateAnswerOptionDto> AnswerOptionsDtos { get; set; } = [];
  }
}

