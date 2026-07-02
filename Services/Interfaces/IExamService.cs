using TestAPI.DTO;
using TestAPI.DTO.Exam;
using TestAPI.DTO.Exam.Requests;
using TestAPI.DTO.ExamAttempt;
using TestAPI.DTO.Question;
using TestAPI.Models.Pagination;
using TestAPI.ResultPattern;

namespace TestAPI.Services.Interfaces
{
    public interface IExamService
    {
        Task<Result<IReadOnlyList<ExamSummaryDto>>> GetAllExamsAsync(CancellationToken ct = default);
        Task<Result<ExamDetailsDto>> GetDetailsByIdAsync(Guid id);
        Task<Result> CreateExamAsync(CreateExamDto dto, CancellationToken ct);
        Task<Result> PublishExam(Guid id);
        Task<Result> DeleteAsync(Guid examId);
        Task<Result> ArchiveAsync(Guid examId);
        Task<Result<IEnumerable<ExamDetailsDto>>> GetPublishedExamsDetailsAsync(PageParameters pageParameters);
        Task<Result<IReadOnlyList<QuestionSnapshotDto>>> CompileExam(Guid examId, CancellationToken ct = default);
        Task<Result> UpdateAsync(Guid id, UpdateExamRequest request);
        Task DeleteRangeAsync(IEnumerable<Guid> examIds);
        Task<Result<IReadOnlyList<ExamOverviewStatsDto>>> GetExamOverviewStatsAsync(CancellationToken ct = default);
    }
}
