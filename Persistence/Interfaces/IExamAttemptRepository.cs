using TestAPI.Entities;

namespace TestAPI.Persistence.Interfaces
{
  public interface IExamAttemptRepository
  {
    public Task AddAsync(ExamAttempt examAttempt);
    public Task<ExamAttempt?> GetByIdAsync(Guid id);

    public Task<int> QueryTotalNumberOfQuestionsBySessionId(Guid sessionId);

    public Task SaveSessionResponses(IEnumerable<UserExamResponse> responses);

    public Task UpdateAsync(ExamAttempt updatedExamAttempt);




    public Task<IEnumerable<ExamAttempt>> GetAllAsync();
    public Task<ExamAttempt?> GetByUserId(Guid userId);
    public Task DeleteAsync(Guid id);
    public Task<IEnumerable<UserExamResponse>> GetResponsesAsync(Guid id);
    public Task<IEnumerable<ExamAttempt>> GetAttemptsByExamAndUserIdAsync(Guid userId, Guid examId, CancellationToken ct);
    public Task<bool> ExistsAsync(Guid id);
  }
}
