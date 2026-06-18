using System.ComponentModel.DataAnnotations;
using TestAPI.DTO.Exam.Requests;
using TestAPI.Enums;

namespace TestAPI.Entities
{
  public class Exam : BaseEntity
  {
    public string Title { get; private set; }
    [MaxLength(500)]
    public string? Context { get; private set; }
    public ExamStatus Status { get; private set; }
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
    [Range(0, 360)]
    public int DurationInMinutes { get; private set; }
    public int NumberOfQuestions { get; private set; }
    public ICollection<Domain>? Domains { get; set; }
    public ICollection<DomainPerformance>? DomainPerfomances { get; set; }
    public ICollection<Question>? Questions { get; set; }

    public Exam(
        Guid categoryId,
        string title,
        string context,
        int durationInMinutes,
        int numberOfQuestions)
    {
      CategoryId = categoryId;
      Title = title;
      Context = context;
      DurationInMinutes = durationInMinutes;
      NumberOfQuestions = numberOfQuestions;
    }

    public void ChangeStatus(ExamStatus examStatus)
    {
      Status = examStatus;
    }

    public void UpdateExamDetails(UpdateExamRequest dto)
    {
      DurationInMinutes = dto.DurationInMinutes;
      NumberOfQuestions = dto.NumberOfQuestions;
    }
  }
}
