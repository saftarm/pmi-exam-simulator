using NuGet.Packaging;
using TestAPI.DTO;
using TestAPI.DTO.ExamAttempt;
using TestAPI.DTO.Question;
using TestAPI.Entities;
using TestAPI.Enums;
using TestAPI.Exceptions;
using TestAPI.Persistence.Implementation;
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
    private readonly IUserExamResponseRepository _userExamResponseRepository;
    private readonly IExamRepository _examRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IProgressService _progressService;
    public ExamAttemptService(
        ILogger<ExamAttemptService> logger,
        IExamService examService,
        IUserRepository userRepository,
        IExamAttemptRepository examAttemptRepository,
        IExamRepository examRepository,
        IQuestionRepository questionRepository,
        IProgressService progressService,
        IUserExamResponseRepository userExamResponseRepository
        )
    {
      _logger = logger;
      _examService = examService;
      _userRepository = userRepository;
      _examAttemptRepository = examAttemptRepository;
      _examRepository = examRepository;
      _questionRepository = questionRepository;
      _progressService = progressService;
      _userExamResponseRepository = userExamResponseRepository;
    }

    public async Task<Result<SessionDto>> StartSession(Guid userId, Guid examId)
    {

      var userExists = await _userRepository.UserExistsByUserId(userId);

      if (!userExists)
      {
        _logger.LogInformation("User with id: {userId} was not found in database while trying to start exam session", userId);
        return Result<SessionDto>.Failure(Errors.UserNotFoundById);
      }

      var totalNumberOfQuestions = await _examRepository.QueryNumberOfQuestionsByExamId(examId);

      // var examExists = await _examRepository.ExamExistsByExamId(examId);

      if (totalNumberOfQuestions == null)
      {
        return Result<SessionDto>.Failure(Errors.ExamNotFound);

      }
      else if (totalNumberOfQuestions == 0)
      {
        return Result<SessionDto>.Failure(Errors.ExamDataCorrupted);
      }

      var newSession = new ExamAttempt(userId: userId, examId: examId, totalQuestions: (int)totalNumberOfQuestions);

      await _examAttemptRepository.AddAsync(newSession);

      var examCompilationResult = await _examService.CompileExam(sessionId: newSession.Id, examId: examId);

      if (!examCompilationResult.IsSuccess)
      {
        return Result<SessionDto>.Failure(Errors.ExamCompilationFailed);
      }
      var sessionDto = new SessionDto
      {
        SessionId = newSession.Id,
        Questions = examCompilationResult.Value!
      };

      return Result<SessionDto>.Success(sessionDto);
    }


    public async Task<Result<SessionResultDto>> FinishSession(FinishSessionRequest request, CancellationToken ct)
    {
      var sessionInDb = await _examAttemptRepository.GetByIdAsync(request.SessionId);

      if (sessionInDb == null)
      {
        return Result<SessionResultDto>.Failure(Errors.RecordNotFoundById);
      }
      
      var sessionResult = await _examService.CalculateSessionResult(sessionInDb.Id, request.SessionResponses);

      if (!sessionResult.IsSuccess)
      {
        return Result<SessionResultDto>.Failure(sessionResult.Error!);
      }

      sessionInDb.FinishSession(
          submittedAt: DateTime.UtcNow,
          status: AttemptStatus.Completed,
          correctCount: sessionResult.Value!.CorrectCount
          );

      await _examAttemptRepository.UpdateAsync(sessionInDb);

      var responses = await _examAttemptRepository.GetResponsesAsync(sessionInDb.Id);
      await _progressService.UpdateDomainPerformanceAsync(
          sessionInDb.UserId,
          sessionInDb.ExamId,
          responses,
          ct);

      return sessionResult;
    }

    public async Task DeleteAsync(Guid id)
    {
      await _examAttemptRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<UserExamResponseDto>> GetResponsesAsync(Guid examAttemptId)
    {
      var userExamResponses = await _examAttemptRepository.GetResponsesAsync(examAttemptId);
      var userExamResponseDtos = userExamResponses.Select(
          r => new UserExamResponseDto
          {
            QuestionId = r.QuestionId,
            SelectedOptionId = r.SelectedOptionId
          }
      );
      return userExamResponseDtos;

    }

  }
}

