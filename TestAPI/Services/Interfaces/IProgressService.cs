using TestAPI.DTO;
using TestAPI.Entities;

namespace TestAPI.Services.Interfaces
{
    public interface IProgressService
    {
        public Task<ExamProgressSummaryDto> GetExamProgressSummaryAsync(Guid userId, Guid examId, CancellationToken ct);
        public Task UpdateDomainPerformance(ExamAttempt examAttempt);
    }
}
