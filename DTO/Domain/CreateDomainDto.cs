

namespace TestAPI.DTO
{
    public class CreateDomainDto
    {
        public Guid ExamId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Weight { get; set; }
    }
}
