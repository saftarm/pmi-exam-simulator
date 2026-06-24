using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestAPI.Entities;

public class DomainPerformance
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Guid UserId { get; set; }
    public Guid ExamId { get; set; }
    public Exam? Exam { get; set; }
    public Guid DomainId { get; set; }
    public Domain? Domain { get; set; }
    public int TotalAnswered { get; set; }
    public decimal TotalCorrect { get; set; }
    public DateTime LastUpdated { get; set; }
}
