namespace TestAPI.DTO.ImportService
{
    public class QuestionImportRowDto
    {
        public int RowNumber { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Explanation { get; set; } 
        public Guid DomainId { get; set; }
        public string? QuestionType { get; set; }
        public List<AnswerOptionImportDto> AnswerOptions { get; set; } = [];
    }
}
