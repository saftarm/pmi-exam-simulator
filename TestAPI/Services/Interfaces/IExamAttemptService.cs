using Microsoft.AspNetCore.Mvc;
using TestAPI.DTO;
using TestAPI.Entities;

namespace TestAPI.Services.Interfaces
{
    public interface IExamAttemptService
    {
        public Task<ExamAttemptDto> GetByIdAsync(int examAttemptId);

        public  Task<ExamAttemptDto> GetByUserId (int userId);

        public Task<int> StartAttemptAsync(int userId, int examId);

        public  Task FinishAttemptAsync(int examAttemptId);

        public Task DeleteAsync(int id);

        public  Task CalculateScore (ExamAttempt examAttempt);

        public Task<int> SaveResponse (int examAttemptId, int questionId, int selectedOptionId );

        public Task<IEnumerable<UserExamResponseDto>> GetResponsesAsync(int examAttemptId);


        // public Task<int> FinishAttempt(FinishExamAttemptRequest request);
        //  public Task<ExamAttempt> FinishAttempt(int examAttemptId);

        
    }
}
