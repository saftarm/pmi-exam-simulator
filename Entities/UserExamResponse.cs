using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestAPI.Entities;

public class UserExamResponse
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public bool IsCorrect { get; private set; }
    public Guid QuestionId { get; private set; }
    public Question? Question { get; set; }
    public Guid DomainId { get; private set; }
    public Domain? Domain { get; set; }
    public Guid SelectedOptionId { get; private set; }
    public AnswerOption? SelectedOption { get; set; }
    public Guid ExamAttemptId { get; private set; }
    public ExamAttempt? ExamAttempt { get; set; }

    public UserExamResponse(
        Guid questionId,
        Guid domainId,
        Guid selectedOptionId,
        Guid examAttemptId,
        bool isCorrect)
    {
        QuestionId = questionId;
        DomainId = domainId;
        SelectedOptionId = selectedOptionId;
        ExamAttemptId = examAttemptId;
        IsCorrect = isCorrect;
    }
}
