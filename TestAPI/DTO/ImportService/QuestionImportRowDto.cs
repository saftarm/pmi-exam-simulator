namespace TestAPI.DTO.ImportService
{
    public class QuestionImportRowDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Explanation { get; set; } 
        public string? DomainName { get; set; }
        public string? QuestionType { get; set; }
        public List<AnswerOptionImportDto> AnswerOptions { get; set; } = [];
    }
}
