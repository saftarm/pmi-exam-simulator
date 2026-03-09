using TestAPI.Entities;

namespace TestAPI.Persistence.Interfaces
{
    public interface IExamAttemptRepository
    {
        public Task<ExamAttempt> GetByIdAsync(int examAttemptId);

        public Task<ExamAttempt> GetByUserId(int userId);

        public Task<int> AddAsync(ExamAttempt examAttempt);

        // public Task<int> UpdateAsync (int examAttemptId, ExamAttempt examAttempt);


    }
}