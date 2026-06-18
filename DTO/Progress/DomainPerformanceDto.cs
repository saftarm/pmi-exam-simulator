namespace TestAPI.DTO.Progress
{
  public class DomainPerformanceDto
  {
    public Guid DomainId { get; set; }
    public string DomainTitle { get; set; } = string.Empty;
    public Guid ExamId { get; set; }
    public string ExamTitle { get; set; } = string.Empty;
    public int TotalAnswered { get; set; }
    public int TotalCorrect { get; set; }
    public decimal PercentageScore { get; set; }
    public DateTime LastUpdated { get; set; }
  }
}
