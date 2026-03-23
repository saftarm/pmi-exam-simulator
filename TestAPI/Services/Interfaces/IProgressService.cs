using TestAPI.DTO;
using TestAPI.DTO.Category;
using TestAPI.Entities;

namespace TestAPI.Services.Interfaces
{
    public interface IProgressService
    {

        // public Task<IEnumerable<ExamAttemptDto>> GetExamHistoryByUserId (int userId);

        public Task UpdateDomainPerformance(ExamAttempt examAttempt);

        public Task GetUserProress();



    }
}
