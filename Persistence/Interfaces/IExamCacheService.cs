using TestAPI.DTO.Question;
using TestAPI.Models.Pagination;
using TestAPI.ResultPattern;

namespace TestAPI.Persistence.Interfaces
{

  public interface IExamCacheService
  {
    public Task<Result> SaveActiveSessionAsync(Guid sessionId, int durationInMinutes, IEnumerable<QuestionDto> compiledQuestions);
    public Task<Result<int>> NumberOfQuestionsBySessionId(Guid sessionId);

    public Task<Result<IEnumerable<QuestionDto>>> QueryPaginatedQuestionsBySessionId(Guid sessionId, PageParameters pageParameters);

  }

}

