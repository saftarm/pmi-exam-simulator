using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestAPI.Entities;

public class Domain
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(100)]
    public string Title { get; private set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Description { get; private set; } = string.Empty;

    [Required]
    [Range(0, 99)]
    public int Weight { get; private set; }

    public Guid ExamId { get; private set; }
    public Exam? Exam { get; set; }

    public ICollection<Question> Questions { get; set; } = [];
    public ICollection<AnswerOption> AnswerOptions { get; set; } = [];
    public ICollection<DomainPerformance> DomainPerformances { get; set; } = [];
    public ICollection<UserExamResponse> UserExamResponses { get; set; } = [];

    public Domain(string title, string description, int weight, Guid examId = default)
    {
        Title = title;
        Description = description;
        Weight = weight;
        ExamId = examId;
    }

    public void Update(string title, string description, int weight)
    {
        Title = title;
        Description = description;
        Weight = weight;
    }
}
