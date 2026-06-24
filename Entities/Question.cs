using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestAPI.Entities;

public class Question
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(1000)]
    public string? Title { get; private set; }

    [MaxLength(1000)]
    public string? Explanation { get; private set; }

    public QuestionType QuestionType { get; private set; }
    public AnswerType AnswerType { get; private set; } = AnswerType.SingleChoice;

    public Guid DomainId { get; private set; }
    public Domain? Domain { get; set; }

    public Guid? ExamId { get; set; }
    public Exam? Exam { get; set; }

    public ICollection<AnswerOption>? AnswerOptions { get; set; }
    public ICollection<UserExamResponse>? UserExamResponses { get; set; }

    public Question(
        string title,
        string explanation,
        QuestionType questionType,
        Guid domainId)
    {
        Title = title;
        Explanation = explanation;
        QuestionType = questionType;
        DomainId = domainId;
        AnswerType = questionType == QuestionType.MultipleChoice
            ? AnswerType.MultipleChoice
            : AnswerType.SingleChoice;
    }

    public void UpdateQuestion(
        string title,
        string explanation,
        QuestionType questionType)
    {
        Title = title;
        Explanation = explanation;
        QuestionType = questionType;
        AnswerType = questionType == QuestionType.MultipleChoice
            ? AnswerType.MultipleChoice
            : AnswerType.SingleChoice;
    }
}
