using TestAPI.DTO.Question;
using TestAPI.Entities;
using TestAPI.Models;

namespace TestAPI.Persistence.Interfaces
{
    public interface IQuestionRepository
    {
        Task AddAsync(Question newQuestion);
        Task AddRangeAsync(IEnumerable<Question> questions);
        Task<int> DeleteRangeAsync(IEnumerable<Guid> questionIds);
        Task UpdateAsync(Question question);
        Task DeleteQuestionById(Guid questionId);
        Task<Question?> GetByIdAsync(Guid questionId);
        Task<Question?> GetByIdWithOptionsAsync(Guid questionId, CancellationToken ct);
        Task<PagedList<QuestionListItemDto>> GetPagedAsync(QuestionQueryParameters query, CancellationToken ct);
        Task<IEnumerable<Question>> QueryQuestionsWithAnswerOptions(Dictionary<Guid, int> domainWeights);
        Task<IEnumerable<AnswerOption>> GetAnswerOptionsByIds(IEnumerable<Guid> optionsIds);
        Task<IReadOnlyList<AnswerOption>> GetAnswerOptionsByQuestionIds(
            IEnumerable<Guid> questionIds,
            CancellationToken ct = default);
        Task<bool> ExistsAsync(Guid questionId, CancellationToken ct);
    }
}
