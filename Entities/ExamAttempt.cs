using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TestAPI.Enums;

namespace TestAPI.Entities;

public class ExamAttempt
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Guid UserId { get; private set; }
    public Guid ExamId { get; private set; }

    [Required]
    [MaxLength(100)]
    public Exam? Exam { get; set; }

    public decimal ScorePoints { get; set; }
    public int TotalQuestions { get; private set; }

    [Range(0, 99)]
    public decimal PercentageScore { get; private set; }

    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? SubmittedAt { get; set; }
    public AttemptStatus Status { get; private set; } = AttemptStatus.InProgress;

    public ICollection<UserExamResponse> UserExamResponses { get; set; } = [];

    public ExamAttempt(Guid userId, Guid examId, int totalQuestions)
    {
        UserId = userId;
        ExamId = examId;
        TotalQuestions = totalQuestions;
    }

    public void FinishSession(DateTime submittedAt, AttemptStatus status, decimal scorePoints)
    {
        SubmittedAt = submittedAt;
        Status = status;
        ScorePoints = scorePoints;
    }

    public void AbandonSession(DateTime submittedAt)
    {
        SubmittedAt = submittedAt;
        Status = AttemptStatus.Abandoned;
    }
}
