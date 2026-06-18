namespace TestAPI.DTO.ExamAttempt
{

  public record SessionResultDto
  {
    public int CorrectCount {get;set;}
    public decimal PercentageScore{get;set;}

  }
}

