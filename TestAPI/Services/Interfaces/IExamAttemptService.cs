using Microsoft.AspNetCore.Mvc;
using TestAPI.DTO;
using TestAPI.Entities;

namespace TestAPI.Services.Interfaces
{
    public interface IExamAttemptService
    {
        public Task<ExamAttemptDto> GetByIdAsync(Guid examAttemptId);

        public  Task<ExamAttemptDto> GetByUserId (Guid userId);

        public Task<Guid> StartAttemptAsync(Guid userId, Guid examId);

        public  Task FinishAttemptAsync(Guid examAttemptId);

        public Task DeleteAsync(Guid id);

        public  Task CalculateScore (ExamAttempt examAttempt);

        public Task<Guid> SaveResponse (Guid examAttemptId, Guid questionId, Guid selectedOptionId );

        // public Task<IEnumerable<UserExamResponseDto>> GetResponsesAsync(Guid examAttemptId);


        // public Task<int> FinishAttempt(FinishExamAttemptRequest request);
        //  public Task<ExamAttempt> FinishAttempt(int examAttemptId);

        
    }
}
