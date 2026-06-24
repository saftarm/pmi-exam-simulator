using TestAPI.DTO.Question;
using TestAPI.Models;
using TestAPI.ResultPattern;

namespace TestAPI.Services.Interfaces
{
    public interface IQuestionService
    {
        Task<Result> CreateQuestionAsync(CreateQuestionDto createQuestionDto);
        Task<Result> DeleteRangeAsync(IEnumerable<Guid> questionIds);
        Task<Result> UpdateAsync(UpdateQuestionRequest request, CancellationToken ct = default);
        Task<Result> DeleteQuestionAsync(Guid questionId, CancellationToken ct = default);
        Task<Result<QuestionAdminDto>> GetByIdAsync(Guid questionId, CancellationToken ct);
        Task<Result<PagedList<QuestionListItemDto>>> GetPagedAsync(QuestionQueryParameters query, CancellationToken ct);
    }
}
