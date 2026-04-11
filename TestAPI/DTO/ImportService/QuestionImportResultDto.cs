namespace TestAPI.DTO.ImportService
{
    public class QuestionImportResultDto
    {
        public bool Success { get; set; }
        public int ImportedCount { get; set; }
        public List<ImportRowErrorDto> Errors { get; set; } = [];
    }
}
