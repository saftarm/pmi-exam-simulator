using TestAPI.Entities;

namespace TestAPI.Persistence.Interfaces
{
  public interface IExamAttemptRepository
  {
    Task AddAsync(ExamAttempt examAttempt);
    Task<ExamAttempt?> GetByIdAsync(Guid id);
    Task<ExamAttempt?> GetByIdForFinishAsync(Guid id, CancellationToken ct);
    Task SaveSessionResponses(IEnumerable<UserExamResponse> responses);
    Task UpdateAsync(ExamAttempt updatedExamAttempt);
    Task<IEnumerable<ExamAttempt>> GetAllAsync();
    Task<ExamAttempt?> GetByUserId(Guid userId);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<UserExamResponse>> GetResponsesAsync(Guid id);
    Task<IEnumerable<ExamAttempt>> GetAttemptsByExamAndUserIdAsync(Guid userId, Guid examId, CancellationToken ct);
    Task<bool> ExistsAsync(Guid id);
  }
}
