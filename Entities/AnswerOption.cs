using System.ComponentModel.DataAnnotations;

namespace TestAPI.Entities
{
  public class AnswerOption : BaseEntity
  {
    public Guid QuestionId { get; private set; }
    public Question? Question { get; set; } 
    public Guid DomainId {get;private set;}
    public Domain? Domain {get;set;}
    [MaxLength(500)]
    public string? Text { get; set; } 
    public bool IsCorrect { get; set; }

    public AnswerOption(
        string text,
        bool isCorrect,
        Guid domainId
        ) {
      Text = text;
      IsCorrect = isCorrect;
      DomainId = domainId;
    }

  }
}
