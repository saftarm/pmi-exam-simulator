using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.Entities;
using TestAPI.Persistence.Interfaces;

namespace TestAPI.Persistence.Implementation
{
  public class ExamAttemptRepository : IExamAttemptRepository
  {
    private readonly ApplicationDbContext _context;

    public ExamAttemptRepository(ApplicationDbContext context)
    {
      _context = context;
    }

    public async Task AddAsync(ExamAttempt examAttempt)
    {
      await _context.ExamAttempts.AddAsync(examAttempt);
      await _context.SaveChangesAsync();
    }

    public async Task SaveSessionResponses(IEnumerable<UserExamResponse> responses)
    {
      await _context.UserExamResponses.AddRangeAsync(responses);
      await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ExamAttempt updatedExamAttempt)
    {
      _context.ExamAttempts.Update(updatedExamAttempt);
      await _context.SaveChangesAsync();
    }

    public async Task<ExamAttempt?> GetByIdForFinishAsync(Guid examAttemptId, CancellationToken ct)
    {
      return await _context.ExamAttempts
          .AsNoTracking()
          .FirstOrDefaultAsync(ea => ea.Id == examAttemptId, ct);
    }

    public async Task<ExamAttempt?> GetByIdAsync(Guid examAttemptId)
    {
      return await _context.ExamAttempts
          .Include(ea => ea.UserExamResponses)
          .FirstOrDefaultAsync(ea => ea.Id == examAttemptId);
    }

    public async Task<IEnumerable<ExamAttempt>> GetAllAsync()
    {
      return await _context.ExamAttempts.ToListAsync();
    }

    public async Task<ExamAttempt?> GetByUserId(Guid userId)
    {
      return await _context.ExamAttempts
          .Include(a => a.UserExamResponses)
          .FirstOrDefaultAsync(e => e.UserId == userId);
    }

    public async Task DeleteAsync(Guid examAttemptId)
    {
      await _context.ExamAttempts
          .Where(a => a.Id == examAttemptId)
          .ExecuteDeleteAsync();
    }

    public async Task<IEnumerable<UserExamResponse>> GetResponsesAsync(Guid examAttemptId)
    {
      return await _context.UserExamResponses
          .Where(r => r.ExamAttemptId == examAttemptId)
          .ToListAsync();
    }

    public async Task<IEnumerable<ExamAttempt>> GetAttemptsByExamAndUserIdAsync(
        Guid userId,
        Guid examId,
        CancellationToken ct)
    {
      return await _context.ExamAttempts
          .Where(a => a.UserId == userId && a.ExamId == examId)
          .ToListAsync(ct);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
      return await _context.ExamAttempts.AnyAsync(a => a.Id == id);
    }
  }
}
