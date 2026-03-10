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

        // public  Task<int> FinishAttemptAsync(int examAttemptId, IEnumerable<UserExamResponse> incomingResponses);

        public  Task<int> CalculateScore (IEnumerable<UserExamResponse> userExamResponses);


        // public Task<int> FinishAttempt(FinishExamAttemptRequest request);
        //  public Task<ExamAttempt> FinishAttempt(int examAttemptId);

        
    }
}
