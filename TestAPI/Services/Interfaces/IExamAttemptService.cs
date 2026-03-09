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

        public Task FinishAttemptAsync(int examAttemptId, UpdatedExamAttemptDto updatedExamAttemptDto);

        public Task<int> CalculateScore (UserExamResponse userExamResponse);
        // public Task<int> FinishAttempt(FinishExamAttemptRequest request);
        //  public Task<ExamAttempt> FinishAttempt(int examAttemptId);

        
    }
}
