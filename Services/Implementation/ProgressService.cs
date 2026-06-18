using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.DTO.Progress;
using TestAPI.Entities;
using TestAPI.Persistence.Interfaces;
using TestAPI.ResultPattern;
using TestAPI.Services.Interfaces;

namespace TestAPI.Services.Implementation
{
  public class ProgressService : IProgressService
  {
    private readonly ApplicationDbContext _context;
    private readonly IUserRepository _userRepository;

    public ProgressService(ApplicationDbContext context, IUserRepository userRepository)
    {
      _context = context;
      _userRepository = userRepository;
    }

    public async Task<Result<IEnumerable<DomainPerformanceDto>>> GetUserDomainPerformancesAsync(Guid userId, CancellationToken ct)
    {
      var userExists = await _userRepository.UserExistsByUserId(userId);
      if (!userExists)
      {
        return Result<IEnumerable<DomainPerformanceDto>>.Failure(Errors.UserNotFoundById);
      }

      var performances = await _context.DomainPerformances
          .AsNoTracking()
          .Include(dp => dp.Domain)
          .Include(dp => dp.Exam)
          .Where(dp => dp.UserId == userId)
          .OrderByDescending(dp => dp.LastUpdated)
          .ToListAsync(ct);

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
        IEnumerable<UserExamResponse> responses,
        CancellationToken ct)
    {
      var statsByDomain = responses
          .GroupBy(r => r.DomainId)
          .ToDictionary(
              g => g.Key,
              g => (CorrectCount: g.Count(r => r.IsCorrect), TotalCount: g.Count()));

      if (statsByDomain.Count == 0)
      {
        return;
      }

      var existingRecords = await _context.DomainPerformances
          .Where(dp => dp.UserId == userId && dp.ExamId == examId)
          .ToListAsync(ct);

      var now = DateTime.UtcNow;

      foreach (var (domainId, stats) in statsByDomain)
      {
        var existing = existingRecords.FirstOrDefault(r => r.DomainId == domainId);

        if (existing == null)
        {
          await _context.DomainPerformances.AddAsync(new DomainPerformance
          {
            UserId = userId,
            ExamId = examId,
            DomainId = domainId,
            TotalAnswered = stats.TotalCount,
            TotalCorrect = stats.CorrectCount,
            LastUpdated = now
          }, ct);
        }
        else
        {
          existing.TotalCorrect += stats.CorrectCount;
          existing.TotalAnswered += stats.TotalCount;
          existing.LastUpdated = now;
        }
      }

      await _context.SaveChangesAsync(ct);
    }
  }
}
