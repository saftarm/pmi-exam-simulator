using TestAPI.DTO;
using TestAPI.DTO.ExamAttempt;
using TestAPI.Entities;
using TestAPI.Enums;
using TestAPI.Persistence.Interfaces;
using TestAPI.ResultPattern;
using TestAPI.Services.Interfaces;

namespace TestAPI.Services.Implementation
{
  public class ExamAttemptService : IExamAttemptService
  {
    private readonly ILogger<ExamAttemptService> _logger;
    private readonly IExamService _examService;
    private readonly IUserRepository _userRepository;
    private readonly IExamAttemptRepository _examAttemptRepository;
    private readonly IExamRepository _examRepository;
    private readonly IProgressService _progressService;

    public ExamAttemptService(
        ILogger<ExamAttemptService> logger,
        IExamService examService,
        IUserRepository userRepository,
        IExamAttemptRepository examAttemptRepository,
        IExamRepository examRepository,
        IProgressService progressService)
    {
      _logger = logger;
      _examService = examService;
      _userRepository = userRepository;
      _examAttemptRepository = examAttemptRepository;
      _examRepository = examRepository;
      _progressService = progressService;
    }

    public async Task<Result<SessionDto>> StartSession(Guid userId, Guid examId)
    {
      var userExists = await _userRepository.UserExistsByUserId(userId);

      if (!userExists)
      {
        _logger.LogInformation(
            "User with id: {UserId} was not found while trying to start exam session",
            userId);
        return Result<SessionDto>.Failure(Errors.UserNotFoundById);
      }

      var totalNumberOfQuestions = await _examRepository.QueryNumberOfQuestionsByExamId(examId);

      if (totalNumberOfQuestions == null)
      {
        return Result<SessionDto>.Failure(Errors.ExamNotFound);
      }

      if (totalNumberOfQuestions == 0)
      {
        return Result<SessionDto>.Failure(Errors.ExamDataCorrupted);
      }

      var newSession = new ExamAttempt(
          userId: userId,
          examId: examId,
          totalQuestions: (int)totalNumberOfQuestions);

      await _examAttemptRepository.AddAsync(newSession);

      var examCompilationResult = await _examService.CompileExam(
          sessionId: newSession.Id,
          examId: examId);

      if (!examCompilationResult.IsSuccess)
      {
        return Result<SessionDto>.Failure(Errors.ExamCompilationFailed);
      }

      return Result<SessionDto>.Success(new SessionDto
      {
        SessionId = newSession.Id,
        Questions = examCompilationResult.Value!
      });
    }

    public async Task<Result<SessionResultDto>> FinishSession(
        FinishSessionRequest request,
        CancellationToken ct)
    {
      var sessionInDb = await _examAttemptRepository.GetByIdForFinishAsync(request.SessionId, ct);

      if (sessionInDb == null)
      {
        return Result<SessionResultDto>.Failure(Errors.RecordNotFoundById);
      }

      var sessionResult = await _examService.CalculateSessionResult(
          sessionInDb.TotalQuestions,
          sessionInDb.Id,
          request.SessionResponses);

      if (!sessionResult.IsSuccess)
      {
        return Result<SessionResultDto>.Failure(sessionResult.Error!);
      }

      sessionInDb.FinishSession(
          submittedAt: DateTime.UtcNow,
          status: AttemptStatus.Completed,
          correctCount: sessionResult.Value!.Result.CorrectCount);

      await _examAttemptRepository.UpdateAsync(sessionInDb);

      await _progressService.UpdateDomainPerformanceAsync(
          sessionInDb.UserId,
          sessionInDb.ExamId,
          sessionResult.Value.SavedResponses,
          ct);

      return Result<SessionResultDto>.Success(sessionResult.Value.Result);
    }

    public async Task DeleteAsync(Guid id)
    {
      await _examAttemptRepository.DeleteAsync(id);
    }
  }
}
