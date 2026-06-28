using StackExchange.Redis;
using TestAPI.DTO;
using TestAPI.DTO.ExamAttempt;
using TestAPI.DTO.Question;
using TestAPI.Entities;
using TestAPI.Enums;
using TestAPI.Models.Session;
using TestAPI.Persistence.Interfaces;
using TestAPI.ResultPattern;
using TestAPI.Services.Interfaces;

namespace TestAPI.Services.Implementation
{
    public class ExamAttemptService : IExamAttemptService
    {
        private const int SessionTtlBufferMinutes = 30;

        private readonly ILogger<ExamAttemptService> _logger;
        private readonly IExamService _examService;
        private readonly IUserRepository _userRepository;
        private readonly IExamAttemptRepository _examAttemptRepository;
        private readonly IExamRepository _examRepository;
        private readonly IDomainPerformanceRepository _domainPerformanceRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISessionSnapshotStore _sessionSnapshotStore;
        private readonly ISessionScoringService _sessionScoringService;

        public ExamAttemptService(
            ILogger<ExamAttemptService> logger,
            IExamService examService,
            IUserRepository userRepository,
            IExamAttemptRepository examAttemptRepository,
            IExamRepository examRepository,
            IDomainPerformanceRepository domainPerformanceRepository,
            IUnitOfWork unitOfWork,
            ISessionSnapshotStore sessionSnapshotStore,
            ISessionScoringService sessionScoringService)
        {
            _logger = logger;
            _examService = examService;
            _userRepository = userRepository;
            _examAttemptRepository = examAttemptRepository;
            _examRepository = examRepository;
            _domainPerformanceRepository = domainPerformanceRepository;
            _unitOfWork = unitOfWork;
            _sessionSnapshotStore = sessionSnapshotStore;
            _sessionScoringService = sessionScoringService;
        }

        public async Task<Result<SessionDto>> StartSession(
            Guid userId,
            Guid examId,
            CancellationToken ct = default)
        {
            var userExists = await _userRepository.UserExistsByUserId(userId);

            if (!userExists)
            {
                _logger.LogInformation(
                    "User with id: {UserId} was not found while trying to start exam session",
                    userId);
                return Result<SessionDto>.Failure(Errors.UserNotFoundById);
            }

            var examStatus = await _examRepository.GetExamStatusByIdAsync(examId);
            if (examStatus is null)
            {
                return Result<SessionDto>.Failure(Errors.ExamNotFound);
            }

            if (examStatus != ExamStatus.Published)
            {
                return Result<SessionDto>.Failure(Errors.ExamNotPublished);
            }

            if (await _examAttemptRepository.HasActiveSessionAsync(userId, examId, ct))
            {
                return Result<SessionDto>.Failure(Errors.SessionAlreadyActive);
            }

            var examCompilationResult = await _examService.CompileExam(examId, ct);
            if (!examCompilationResult.IsSuccess)
            {
                return Result<SessionDto>.Failure(examCompilationResult.Error!);
            }

            var compiledQuestions = examCompilationResult.Value!.ToList();
            if (compiledQuestions.Count == 0)
            {
                return Result<SessionDto>.Failure(Errors.ExamDataCorrupted);
            }

            var exam = await _examRepository.GetByIdAsync(examId);
            if (exam is null)
            {
                return Result<SessionDto>.Failure(Errors.ExamNotFound);
            }

            var newSession = new ExamAttempt(
                userId: userId,
                examId: examId,
                totalQuestions: compiledQuestions.Count);

            await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                await _examAttemptRepository.AddAsync(newSession);
                await _unitOfWork.CommitTransactionAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to persist exam attempt for user {UserId}", userId);
                await _unitOfWork.RollbackTransactionAsync(ct);
                return Result<SessionDto>.Failure(Errors.SessionPersistenceFailed);
            }

            var snapshot = BuildSessionSnapshot(newSession.Id, examId, userId, compiledQuestions);
            var ttl = TimeSpan.FromMinutes(exam.DurationInMinutes + SessionTtlBufferMinutes);

            try
            {
                await _sessionSnapshotStore.SaveAsync(snapshot, ttl, ct);
            }
            catch (Exception ex) when (ex is RedisConnectionException or RedisTimeoutException or InvalidOperationException)
            {
                _logger.LogError(ex, "Failed to persist session snapshot for {SessionId}", newSession.Id);
                await _examAttemptRepository.DeleteAsync(newSession.Id);
                return Result<SessionDto>.Failure(Errors.SessionStoreUnavailable);
            }

            return Result<SessionDto>.Success(new SessionDto
            {
                SessionId = newSession.Id,
                Questions = compiledQuestions,
            });
        }

        public async Task<Result<SessionResultDto>> FinishSession(
            FinishSessionRequest request,
            Guid actingUserId,
            CancellationToken ct)
        {
            var sessionInDb = await _examAttemptRepository.GetByIdForFinishAsync(request.SessionId, ct);

            if (sessionInDb == null)
            {
                return Result<SessionResultDto>.Failure(Errors.RecordNotFoundById);
            }

            if (sessionInDb.UserId != actingUserId)
            {
                return Result<SessionResultDto>.Failure(Errors.SessionNotOwned);
            }

            if (sessionInDb.Status != AttemptStatus.InProgress)
            {
                return Result<SessionResultDto>.Failure(Errors.SessionAlreadyCompleted);
            }

            SessionSnapshot? snapshot;
            try
            {
                snapshot = await _sessionSnapshotStore.GetAsync(request.SessionId, ct);
            }
            catch (Exception ex) when (ex is RedisConnectionException or RedisTimeoutException)
            {
                _logger.LogError(ex, "Failed to load session snapshot for {SessionId}", request.SessionId);
                return Result<SessionResultDto>.Failure(Errors.SessionStoreUnavailable);
            }

            if (snapshot is null)
            {
                return Result<SessionResultDto>.Failure(Errors.SessionExpired);
            }

            var sessionResult = await _sessionScoringService.ValidateAndScoreAsync(
                snapshot,
                actingUserId,
                sessionInDb.Id,
                request.SessionResponses,
                ct);

            if (!sessionResult.IsSuccess)
            {
                return Result<SessionResultDto>.Failure(sessionResult.Error!);
            }

            await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                await _examAttemptRepository.SaveSessionResponses(sessionResult.Value!.SavedResponses);

                sessionInDb.FinishSession(
                    submittedAt: DateTime.UtcNow,
                    status: AttemptStatus.Completed,
                    scorePoints: sessionResult.Value.Result.ScorePoints);

                await _examAttemptRepository.UpdateAsync(sessionInDb);

                await _domainPerformanceRepository.UpsertSessionStatsAsync(
                    sessionInDb.UserId,
                    sessionInDb.ExamId,
                    sessionResult.Value.DomainStats,
                    ct);

                await _unitOfWork.CommitTransactionAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to complete session {SessionId}", request.SessionId);
                await _unitOfWork.RollbackTransactionAsync(ct);
                return Result<SessionResultDto>.Failure(Errors.ExamCompilationFailed);
            }

            try
            {
                await _sessionSnapshotStore.DeleteAsync(request.SessionId, ct);
            }
            catch (Exception ex) when (ex is RedisConnectionException or RedisTimeoutException)
            {
                _logger.LogWarning(ex, "Failed to delete session snapshot after finish for {SessionId}", request.SessionId);
            }

            return Result<SessionResultDto>.Success(sessionResult.Value.Result);
        }

        public async Task<Result> AbandonSessionAsync(
            Guid sessionId,
            Guid actingUserId,
            CancellationToken ct = default)
        {
            var sessionInDb = await _examAttemptRepository.GetByIdForFinishAsync(sessionId, ct);

            if (sessionInDb == null)
            {
                return Result.Failure(Errors.RecordNotFoundById);
            }

            if (sessionInDb.UserId != actingUserId)
            {
                return Result.Failure(Errors.SessionNotOwned);
            }

            if (sessionInDb.Status != AttemptStatus.InProgress)
            {
                return Result.Failure(Errors.SessionNotInProgress);
            }

            sessionInDb.AbandonSession(DateTime.UtcNow);
            await _examAttemptRepository.UpdateAsync(sessionInDb);
            await _unitOfWork.SaveChangesAsync(ct);

            try
            {
                await _sessionSnapshotStore.DeleteAsync(sessionId, ct);
            }
            catch (Exception ex) when (ex is RedisConnectionException or RedisTimeoutException)
            {
                _logger.LogWarning(ex, "Failed to delete session snapshot for abandoned session {SessionId}", sessionId);
            }

            return Result.Success();
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            if (!await _examAttemptRepository.ExistsAsync(id))
            {
                return Result.Failure(Errors.RecordNotFoundById);
            }

            await _examAttemptRepository.DeleteAsync(id);

            try
            {
                await _sessionSnapshotStore.DeleteAsync(id);
            }
            catch (Exception ex) when (ex is RedisConnectionException or RedisTimeoutException)
            {
                _logger.LogWarning(ex, "Failed to delete session snapshot for removed attempt {SessionId}", id);
            }

            return Result.Success();
        }

        private static SessionSnapshot BuildSessionSnapshot(
            Guid sessionId,
            Guid examId,
            Guid userId,
            IReadOnlyList<QuestionSnapshotDto> compiledQuestions)
        {
            return new SessionSnapshot
            {
                SessionId = sessionId,
                ExamId = examId,
                UserId = userId,
                TotalQuestions = compiledQuestions.Count,
                Questions = compiledQuestions
                    .Select((question, index) => new SessionQuestionEntry
                    {
                        QuestionId = question.Id,
                        QuestionType = question.QuestionType,
                        OrderIndex = index,
                    })
                    .ToList(),
            };
        }
    }
}
