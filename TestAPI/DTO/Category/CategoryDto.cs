namespace TestAPI.DTO
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public IEnumerable<ExamSummaryDto>? ExamSummaryDtos { get; set; }
    }
}

