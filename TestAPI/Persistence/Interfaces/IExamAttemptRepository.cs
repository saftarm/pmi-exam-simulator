using TestAPI.DTO;
using TestAPI.Entities;

namespace TestAPI.Persistence.Interfaces
{
    public interface IExamAttemptRepository
    {
        public Task<ExamAttempt> GetByIdAsync(Guid examAttemptId);

        public Task<IEnumerable<ExamAttempt>> GetAllAsync();

        public Task<ExamAttempt> GetByUserId(Guid userId);

        public Task<Guid> AddAsync(ExamAttempt examAttempt);

        // public Task<IEnumerable<AnswerOption>> GetAnswerOptionsAsync();

        public Task<Guid> UpdateAsync (ExamAttempt updatedExamAttempt);

        public Task DeleteAsync(Guid examAttemptId);

        // public Task<AnswerOption> GetAnswerOptionAsync(Guid selectedOptionId);

        public  Task<IEnumerable<UserExamResponse>> GetResponsesAsync(Guid examAttemptId);




    }
}