using TestAPI.DTO.Progress;
using TestAPI.Entities;
using TestAPI.Persistence.Interfaces;
using TestAPI.ResultPattern;
using TestAPI.Services.Interfaces;

namespace TestAPI.Services.Implementation
{
    public class ProgressService : IProgressService
    {
        private readonly IDomainPerformanceRepository _domainPerformanceRepository;
        private readonly IUserRepository _userRepository;

        public ProgressService(
            IDomainPerformanceRepository domainPerformanceRepository,
            IUserRepository userRepository)
        {
            _domainPerformanceRepository = domainPerformanceRepository;
            _userRepository = userRepository;
        }

        public async Task<Result<IEnumerable<DomainPerformanceDto>>> GetUserDomainPerformancesAsync(Guid userId, CancellationToken ct)
        {
            var userExists = await _userRepository.UserExistsByUserId(userId);
            if (!userExists)
            {
                return Result<IEnumerable<DomainPerformanceDto>>.Failure(Errors.UserNotFoundById);
            }

            var performances = await _domainPerformanceRepository.GetByUserIdWithDetailsAsync(userId, ct);

            var performanceDtos = performances.Select(dp => new DomainPerformanceDto
            {
                DomainId = dp.DomainId,
                DomainTitle = dp.Domain?.Title ?? string.Empty,
                ExamId = dp.ExamId,
                ExamTitle = dp.Exam?.Title ?? string.Empty,
                TotalAnswered = dp.TotalAnswered,
                TotalCorrect = dp.TotalCorrect,
                PercentageScore = dp.TotalAnswered == 0
                  ? 0
                  : Math.Round((decimal)dp.TotalCorrect / dp.TotalAnswered * 100, 2),
                LastUpdated = dp.LastUpdated
            });

            return Result<IEnumerable<DomainPerformanceDto>>.Success(performanceDtos);
        }

        public async Task UpdateDomainPerformanceAsync(
            Guid userId,
            Guid examId,
            IReadOnlyDictionary<Guid, (decimal ScorePoints, int QuestionCount)> statsByDomain,
            CancellationToken ct)
        {
            await _domainPerformanceRepository.UpsertSessionStatsAsync(userId, examId, statsByDomain, ct);
        }
    }
}
