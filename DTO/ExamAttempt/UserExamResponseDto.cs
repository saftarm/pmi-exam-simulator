namespace TestAPI.DTO
{
    public class UserExamResponseDto
    {
        public Guid QuestionId { get; set; }
        public  IReadOnlyList<Guid> SelectedOptionIds { get; set; } = [];
    }
}

