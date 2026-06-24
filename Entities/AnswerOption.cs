using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestAPI.Entities;

public class AnswerOption
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Guid QuestionId { get; private set; }
    public Question? Question { get; set; }

    public Guid DomainId { get; private set; }
    public Domain? Domain { get; set; }

    [MaxLength(500)]
    public string? Text { get; set; }

    public bool IsCorrect { get; set; }

    public AnswerType AnswerType { get; set; } = AnswerType.SingleChoice;

    public AnswerOption(
        string text,
        bool isCorrect,
        Guid domainId)
    {
        Text = text;
        IsCorrect = isCorrect;
        DomainId = domainId;
    }
}
