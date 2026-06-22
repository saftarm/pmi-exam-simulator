namespace TestAPI.DTO.Exam
{
  public record ExamDetailsDto
  {
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Context { get; set; }
    public int DurationInMinutes { get; set; }
    public int NumberOfQuestions { get; set; }
    public int AttemptCount { get; set; }
    public bool IsMostPopular { get; set; }
  }
}

