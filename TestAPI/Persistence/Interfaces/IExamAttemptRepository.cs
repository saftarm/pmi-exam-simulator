using TestAPI.Entities;

namespace TestAPI.Persistence.Interfaces
{
    public interface IExamAttemptRepository
    {
        public Task<ExamAttempt> GetByIdAsync(int examAttemptId);

        public Task<ExamAttempt> GetByUserId(int userId);

        public Task<int> AddAsync(ExamAttempt examAttempt);

        public Task<IEnumerable<AnswerOption>> GetAnswerOptionsAsync();

        public Task<int> Update (ExamAttempt updatedExamAttempt);

        public Task<AnswerOption> GetAnswerOptionAsync(int selectedOptionId);

        public  Task<IEnumerable<UserExamResponse>> GetResponsesAsync(int examAttemptId);
        public Task<IEnumerable<AnswerOption>> GetAnswerOptionsByExamId( int examId );


    }
}