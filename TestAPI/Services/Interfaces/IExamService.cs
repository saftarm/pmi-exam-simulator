using TestAPI.DTO;
using TestAPI.DTO.Exam.Responses;
using TestAPI.Models;

namespace TestAPI.Services.Interfaces
{
    public interface IExamService
    {
        public Task<IEnumerable<CreateExamResponse>> CreateExams(List<CreateExamDto> dto);
        public Task CompileExam(Guid id);
        public Task<IEnumerable<ExamSummaryDto>> GetSummariesAsync(PageParameters pageParameters);
        public Task<ExamSummaryDto> GetSummaryByIdAsync(Guid id);
        public Task<ExamDetailsDto> GetDetailsByIdAsync(Guid id);
        public Task<IEnumerable<ExamDetailsDto>> GetDetailsAsync(PageParameters pageParameters);
        public Task<ExamFullDto> GetByIdAsync(Guid id);
        public Task DeleteAsync(Guid examId);
        public Task DeleteRangeAsync(ICollection<Guid> examIds);
        public Task<int> CalculateScore(Guid examAttemptId);
        public Task AddQuestionsToExamAsync(Guid examId, ICollection<Guid> questionIds);

        public Task PublihExam(Guid id);

        public Task<IEnumerable<ExamSummaryDto>> GetPublishedExamSummariesByCategoryId(Guid id, PageParameters pageParameters, CancellationToken ct);





    }
}
