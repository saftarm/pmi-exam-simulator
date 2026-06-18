using TestAPI.Entities;
namespace TestAPI.Persistence.Interfaces
{
  public interface IQuestionRepository
  {
    // Basic CRUD
    Task<int> AddAsync(Question newQuestion); // create questions
    Task AddRangeAsync(IEnumerable<Question> questions); // create multiple questions
    Task<int> DeleteRangeAsync(IEnumerable<Guid> questionIds); // delete multiple queustions
    Task<int> UpdateAsync(Question question); // update question
    Task DeleteQuestionById(Guid questionId); // delete single question
    Task<Question?> GetByIdAsync(Guid questionId);


    // Queries
    Task<IEnumerable<Question>> QueryQuestionsWithAnswerOptions(Dictionary<Guid, int> domainWeights);

    // AnswerOptions

    Task<IEnumerable<AnswerOption>> GetAnswerOptionsByIds(IEnumerable<Guid> optionsIds);

    // Checks
    Task<bool> ExistsAsync(Guid questionId, CancellationToken ct);
  }
}
