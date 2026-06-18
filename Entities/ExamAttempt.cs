using System.ComponentModel.DataAnnotations;
using TestAPI.Enums;
namespace TestAPI.Entities
{
  public class ExamAttempt : BaseEntity
  {
    public Guid UserId { get; private set; }
    public Guid ExamId { get; private set; }

    [Required]
    [MaxLength(100)]
    public Exam? Exam { get; set; }
    public int CorrectCount { get; set; } = 0;
    public int TotalQuestions { get; private set; }
    [Range(0, 99)]
    public decimal PercentageScore {get; private set;} // Computed Column
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? SubmittedAt { get; set; }
    public AttemptStatus Status { get; private set; } = AttemptStatus.InProgress;
    public ICollection<UserExamResponse> UserExamResponses { get; set; } = [];

    public ExamAttempt(
        Guid userId,
        Guid examId,
        int totalQuestions)
    {
      UserId = userId;
      ExamId = examId;
      TotalQuestions = totalQuestions;
    }

    public void FinishSession(
        DateTime submittedAt,
        AttemptStatus status,
        int correctCount
        )
    {
      SubmittedAt = submittedAt;
      Status = status;
      CorrectCount = correctCount;
    }

    public void ChangeSessionStatus(AttemptStatus attemptStatus)
    {
      Status = attemptStatus;
    }
  }
}
