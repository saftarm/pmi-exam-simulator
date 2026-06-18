using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.DTO;
using TestAPI.Entities;
using TestAPI.Persistence.Interfaces;

namespace TestAPI.Persistence.Implementation
{
  public class QuestionRepository(ApplicationDbContext context) : IQuestionRepository
  {
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<Question>> QueryQuestionsWithAnswerOptions(Dictionary<Guid, int> numberOfQuestionsPerDomain)
    {
      var allQuestions = new List<Question>();

      foreach (var (domainId, numberOfQuestions) in numberOfQuestionsPerDomain)
      {
        var questions = _context.Questions
          .AsNoTracking()
          .Where(q => q.DomainId == domainId)
          .Include(q => q.AnswerOptions)
          .OrderBy(q => EF.Functions.Random())
          .Take(numberOfQuestions)
          .AsEnumerable();

        allQuestions.AddRange(questions);
      }

      return allQuestions;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken ct)
    {
      return await _context.Questions.AnyAsync(q => q.Id == id, ct);
    }
    // Add Question
    public async Task<int> AddAsync(Question newQuestion)
    {
      await _context.Questions.AddAsync(newQuestion);
      return await _context.SaveChangesAsync();
    }

    public async Task<int> DeleteRangeAsync(IEnumerable<Guid> questionIds)
    {
      return await _context.Questions
        .Where(q => questionIds.Contains(q.Id))
        .ExecuteDeleteAsync();
    }

    // Update question async
    public async Task<int> UpdateAsync(Question question)
    {
      _context.Questions.Update(question);
      return await _context.SaveChangesAsync();
    }

    // Add range of questions
    public async Task AddRangeAsync(IEnumerable<Question> questions)
    {
      _context.Questions.AddRange(questions);
      await _context.SaveChangesAsync();
    }
    // Delete
   
    public async Task DeleteQuestionById(Guid questionId) {
      await _context.Questions
        .Where(q => q.Id == questionId)
        .ExecuteDeleteAsync();
    }


    // Get By Id
    public async Task<Question?> GetByIdAsync(Guid questionId)
    {
      return await _context.Questions.FindAsync(questionId);
    }

    public async Task<IEnumerable<AnswerOption>> GetAnswerOptionsByIds(IEnumerable<Guid> optionsIds)
    {
      return await _context.AnswerOptions
        .Where(o => optionsIds.Contains(o.Id))
        .ToListAsync();
    }

    public async Task<int> GetNumberOfQuestionsByDomainId(Guid domainId)
    {
      return await _context.Questions.CountAsync(q => q.DomainId == domainId);
    }

  }

}
