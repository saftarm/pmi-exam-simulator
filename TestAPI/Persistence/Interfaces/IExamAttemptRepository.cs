using TestAPI.Entities;

namespace TestAPI.Persistence.Interfaces
{
    public interface IExamAttemptRepository
    {
        public Task<ExamAttempt?> GetByIdAsync(Guid id);
        public Task<IEnumerable<ExamAttempt>> GetAllAsync();
        public Task<ExamAttempt?> GetByUserId(Guid userId);
        public Task<Guid> AddAsync(ExamAttempt examAttempt);
        public Task<Guid> UpdateAsync(ExamAttempt updatedExamAttempt);
        public Task DeleteAsync(Guid id);
        public Task<IEnumerable<UserExamResponse>> GetResponsesAsync(Guid id);
        public Task<IEnumerable<ExamAttempt>> GetAttemptsByExamAndUserIdAsync(Guid userId, Guid examId, CancellationToken ct);

        public Task<bool> ExistsAsync(Guid id);

        //public Task<string?> GetExamTitleByExamIdAsync(Guid examId, CancellationToken ct);




    }
}