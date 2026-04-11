using TestAPI.DTO.ImportService;

namespace TestAPI.Services.Interfaces
{
    public interface IQuestionImportService
    {
        public Task<QuestionImportResultDto> ImportFromExcelAsync(IFormFile file, CancellationToken ct);
        //public byte[] GenerateTemplate();

    }
}
