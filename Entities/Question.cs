using System.ComponentModel.DataAnnotations;

namespace TestAPI.Entities
{
  public class Question : BaseEntity
  {
    [MaxLength(1000)]
    public string? Title { get; private set; } 
    [MaxLength(1000)]
    public string? Explanation { get; private set; } 
    public QuestionType QuestionType { get; private set; }
    public Guid DomainId { get; private set; }
    public Domain? Domain { get; set; }
    public Guid? ExamId { get; set; }
    public Exam? Exam { get; set; } = null;
    public ICollection<AnswerOption>? AnswerOptions { get; set; }
    public ICollection<UserExamResponse>? UserExamResponses { get; set; } 

    public Question(
        string title,
        string explanation,
        QuestionType questionType,
        Guid domainId
        ) {
      Title = title;
      Explanation = explanation;
      QuestionType = questionType;
      DomainId = domainId;
    }

    public void UpdateQuestion(
        string title,
        string explanation,
        QuestionType questionType
        ) {
      Title = title;
      Explanation = explanation;
      QuestionType = questionType;
    }

  }
}
