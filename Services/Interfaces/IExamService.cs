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
    public Task<IEnumerable<Exam>> GetAllExams(); // Get all exams
    public Task<Result<ExamDetailsDto>> GetDetailsByIdAsync(Guid id); // Get Exam details by id
    public Task<Result> CreateExamAsync(CreateExamDto dto); // Create Exam
    public Task<Result> PublishExam(Guid id); // Publish Exam
    public Task DeleteAsync(Guid examId); // Hard delete Exam
    public Task<Result> ArchiveAsync(Guid examId); // Archive Exam
    public Task<Result<IEnumerable<ExamDetailsDto>>> GetPublishedExamsDetailsAsync(PageParameters pageParameters); // Get details of published Exams

    public Task<Result<IEnumerable<QuestionDto>>> CompileExam(Guid sessionId, Guid examId);
    public Task<Result<SessionResultDto>> CalculateSessionResult(Guid sessionId, IEnumerable<UserExamResponseDto> responses);

    // ------------------------------------- Needs refactoring -------------------------------------------------
    public Task<Result> UpdateAsync(Guid id, UpdateExamRequest request);
    public Task DeleteRangeAsync(IEnumerable<Guid> examIds);
  }
}
