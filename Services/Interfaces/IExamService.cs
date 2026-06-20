using TestAPI.DTO;
using TestAPI.DTO.Exam;
using TestAPI.DTO.Exam.Requests;
using TestAPI.DTO.ExamAttempt;
using TestAPI.DTO.Question;
using TestAPI.Entities;
using TestAPI.Models.Pagination;
using TestAPI.ResultPattern;

namespace TestAPI.Services.Interfaces
{
  public interface IExamService
  {
    Task<IEnumerable<Exam>> GetAllExams();
    Task<Result<ExamDetailsDto>> GetDetailsByIdAsync(Guid id);
    Task<Result> CreateExamAsync(CreateExamDto dto);
    Task<Result> PublishExam(Guid id);
    Task DeleteAsync(Guid examId);
    Task<Result> ArchiveAsync(Guid examId);
    Task<Result<IEnumerable<ExamDetailsDto>>> GetPublishedExamsDetailsAsync(PageParameters pageParameters);
    Task<Result<IEnumerable<QuestionDto>>> CompileExam(Guid sessionId, Guid examId);
    Task<Result<SessionCalculationResult>> CalculateSessionResult(
        int totalQuestionsInSession,
        Guid sessionId,
        IEnumerable<UserExamResponseDto> responses);
    Task<Result> UpdateAsync(Guid id, UpdateExamRequest request);
    Task DeleteRangeAsync(IEnumerable<Guid> examIds);
  }
}
