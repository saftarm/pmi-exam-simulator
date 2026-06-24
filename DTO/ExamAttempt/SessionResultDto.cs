namespace TestAPI.DTO.ExamAttempt
{

    public record SessionResultDto
    {
        public decimal ScorePoints { get; set; }
        public decimal PercentageScore { get; set; }

    }
}

