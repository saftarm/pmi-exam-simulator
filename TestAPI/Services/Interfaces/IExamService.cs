using Microsoft.AspNetCore.Mvc;
using TestAPI.DTO;
using TestAPI.Entities;
using TestAPI.Models;

namespace TestAPI.Services.Interfaces
{
    public interface IExamService
    {
        public Task CreateExams(List<CreateExamDto> dto);
        // public Task CompileExam(int examId);

        public Task<IEnumerable<PublishedExamDto>> GetPublishedExamsAsync(PageParameters pageParemeters);
        public Task<IEnumerable<ExamSummaryDto>> GetSummariesAsync(PageParameters pageParameters);
        public Task<ExamSummaryDto> GetSummaryByIdAsync(int id);
        public Task<ExamDetailsDto> GetDetailsByIdAsync(int id);
        public Task<IEnumerable<ExamDetailsDto>> GetDetailsAsync(PageParameters pageParameters);

        public Task<ExamFullDto> GetByIdAsync(int id);
        public Task DeleteAsync(int examId);
        public Task DeleteRangeAsync(ICollection<int> examIds);
        public Task<int> CalculateScore(int examAttemptId);
        public Task AddQuestionsToExamAsync(int examId, ICollection<int> questionIds);



    }
}
