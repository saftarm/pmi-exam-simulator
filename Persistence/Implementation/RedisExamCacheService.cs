using System.Text.Json;
using StackExchange.Redis;
using TestAPI.DTO.Question;
using TestAPI.Persistence.Interfaces;
using TestAPI.ResultPattern;
using System.Threading.Tasks;
using TestAPI.Models.Pagination;
using DocumentFormat.OpenXml.Wordprocessing;

namespace TestAPI.Persistence.Implementation
{

  public class RedisExamCacheService : IExamCacheService
  {
    private readonly IDatabase _redisDb;

    public RedisExamCacheService(IConnectionMultiplexer redisDb)
    {
      _redisDb = redisDb.GetDatabase();
    }

    public async Task<Result> SaveActiveSessionAsync(Guid sessionId, int durationInMinutes, IEnumerable<QuestionDto> compiledQuestions)
    {

      var key = $"sessionId:{sessionId}";
      var serializedData = JsonSerializer.Serialize(compiledQuestions);
      await _redisDb.StringSetAsync(key, serializedData, TimeSpan.FromMinutes(durationInMinutes));
      return Result.Success();
    }


    public async Task<Result<int>> NumberOfQuestionsBySessionId(Guid sessionId)
    {
      var key = $"sessionId:{sessionId}";

      var sessionExists = await _redisDb.KeyExistsAsync(key);
      if(!sessionExists) {
        return Result<int>.Failure(Errors.RedisKeyNotFound);
      }

      RedisValue questions = await _redisDb.StringGetAsync(key);

      if (questions.IsNullOrEmpty)
      {
        return Result<int>.Failure(Errors.RangeOfRecordsNotFound);
      }

      var questionsList = JsonSerializer.Deserialize<IEnumerable<QuestionDto>>((byte[])questions);

      return Result<int>.Success(questionsList!.Count());
    }

    public async Task<Result<IEnumerable<QuestionDto>>> QueryPaginatedQuestionsBySessionId(Guid sessionId, PageParameters pageParameters)
    {
      var key = $"sessionId:{sessionId}";
      RedisValue questions = await _redisDb.StringGetAsync(key);
      if (questions.IsNullOrEmpty)
      {
        return Result<IEnumerable<QuestionDto>>.Failure(Errors.RangeOfRecordsNotFound);
      }
      var questionsList = JsonSerializer.Deserialize<IEnumerable<QuestionDto>>((byte[])questions);
      var paginatedQuestions = questionsList!.
        Skip(pageParameters.PageSize * (pageParameters.PageNumber - 1 )).
        Take(pageParameters.PageSize)
        .ToList();

      return Result<IEnumerable<QuestionDto>>.Success(paginatedQuestions);


    }

  }
}
