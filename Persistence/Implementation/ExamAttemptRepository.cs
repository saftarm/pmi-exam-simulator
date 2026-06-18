



using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.Entities;
using TestAPI.Exceptions;
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

    // Creating new Exam Session 
    public async Task AddAsync(ExamAttempt examAttempt)
    {
      await _context.ExamAttempts.AddAsync(examAttempt);
      await _context.SaveChangesAsync();
    }

    public async Task SaveSessionResponses(IEnumerable<UserExamResponse> responses) {

      await _context.UserExamResponses.AddRangeAsync(responses);
      await _context.SaveChangesAsync();

    }


    // Update Exam Session 
    public async Task UpdateAsync(ExamAttempt updatedExamAttempt)
    {
      _context.ExamAttempts.Update(updatedExamAttempt);
      await _context.SaveChangesAsync();
    }

    // Getting Attempt with eager downloading
    public async Task<ExamAttempt?> GetByIdAsync(Guid examAttemptId)
    {
      var examAttempt = await _context.ExamAttempts
      .Include(ea => ea.UserExamResponses)
      .Include(ea => ea.Exam)
          .ThenInclude(e => e.Domains)
              .ThenInclude(e => e.Questions)
      .FirstOrDefaultAsync(ea => ea.Id == examAttemptId);

      return examAttempt;
    }

    public async Task<int> QueryTotalNumberOfQuestionsBySessionId(Guid sessionId)
    {
      return await _context.ExamAttempts
        .Where(s => s.Id == sessionId)
        .Select(s => s.Exam != null ? s.Exam.NumberOfQuestions : 0)
        .FirstOrDefaultAsync();

    }
    public async Task<IEnumerable<ExamAttempt>> GetAllAsync()
    {
      return await _context.ExamAttempts.ToListAsync();
    }

    public async Task<ExamAttempt?> GetByUserId(Guid userId)
    {
      var examAttempt = await _context.ExamAttempts
      .Include(a => a.UserExamResponses)
      .FirstOrDefaultAsync(e => e.UserId == userId);
      return examAttempt;
    }
    // Hard delete Attempt by id
    public async Task DeleteAsync(Guid examAttemptId)
    {
      var examAttempt = await _context.ExamAttempts
      .Where(a => a.Id == examAttemptId)
      .ExecuteDeleteAsync();
    }

    // Get Responses by Attempt id
    public async Task<IEnumerable<UserExamResponse>> GetResponsesAsync(Guid examAttemptId)
    {
      return await _context.UserExamResponses
      .Where(r => r.ExamAttemptId == examAttemptId)
      .ToListAsync();
    }

    // Getting attempts of user on specific exam
    public async Task<IEnumerable<ExamAttempt>> GetAttemptsByExamAndUserIdAsync(Guid userId, Guid examId, CancellationToken ct)
    {
      return await _context.ExamAttempts
          .Where(a => a.UserId == userId && a.ExamId == examId)
          .ToListAsync();
    }

    // Checking if ExamAttempt exists by id
    public async Task<bool> ExistsAsync(Guid id)
    {
      return await _context.ExamAttempts.AnyAsync(a => a.Id == id);
    }

  }
}


