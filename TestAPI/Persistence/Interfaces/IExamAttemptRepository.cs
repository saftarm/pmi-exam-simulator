using TestAPI.DTO;
using TestAPI.Entities;

namespace TestAPI.Persistence.Interfaces
{
    public interface IExamAttemptRepository
    {
        public Task<ExamAttempt> GetByIdAsync(int examAttemptId);

        public Task<IEnumerable<ExamAttempt>> GetAllAsync();

        public Task<ExamAttempt> GetByUserId(int userId);

        public Task<int> AddAsync(ExamAttempt examAttempt);

        public Task<IEnumerable<AnswerOption>> GetAnswerOptionsAsync();

        public Task<int> UpdateAsync (ExamAttempt updatedExamAttempt);

        public Task DeleteAsync(int examAttemptId);

        public Task<AnswerOption> GetAnswerOptionAsync(int selectedOptionId);

        public  Task<IEnumerable<UserExamResponse>> GetResponsesAsync(int examAttemptId);




    }
}