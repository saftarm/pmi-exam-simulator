using Microsoft.AspNetCore.Mvc;
using TestAPI.DTO;
using TestAPI.Entities;

namespace TestAPI.Services.Interfaces
{
    public interface IExamService
    {
        public Task CreateExam(CreateExamDto createExamDto);
        public Task CompileExam(CompileExamDto compileExamDto);
        public Task<IEnumerable<ExamSummaryDto>> GetSummaryAsync();
        public Task<ExamDetailsDto> GetDetailsByIdAsync(int examId);
        public Task<ExamSummaryDto> GetByIdAsync(int examId);
        public Task DeleteAsync(int examId);
        public Task DeleteRangeAsync(ICollection<int> examIds);
        public Task<int> CalculateScore(int examAttemptId);
        public Task AddQuestionsToExamAsync(int examId, ICollection<int> questionIds);



    }
}
