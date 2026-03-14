namespace TestAPI.DTO
{
    public class CreateCategoryDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public ICollection<int>? ExamIds { get; set; }
    }
}

