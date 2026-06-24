using TestAPI.DTO.ImportService;
using TestAPI.ResultPattern;

namespace TestAPI.Services.Interfaces
{
    public interface IQuestionImportService
    {
        public Task<Result<QuestionImportResultDto>> ImportFromExcelAsync(IFormFile file, CancellationToken ct);
    }
}
