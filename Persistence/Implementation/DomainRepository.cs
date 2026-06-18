using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.Entities;
using TestAPI.Exceptions;
using TestAPI.Persistence.Interfaces;
using TestAPI.ResultPattern;

namespace TestAPI.Persistence.Implementation
{
  public class DomainRepository : IDomainRepository
  {
    private readonly ApplicationDbContext _context;
    public DomainRepository(ApplicationDbContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<Domain>> GetDomainsByExamIdAsync(Guid examId)
   {
      return await _context.Domains
        .Where(d => d.ExamId == examId)
        .ToListAsync();

    }
    // Get Domain by Id
    public async Task<Domain?> GetByIdAsync(Guid id)
    {
      return await _context.Domains.FindAsync(id);
    }

    // Get all domains
    public async Task<IEnumerable<Domain>> GetAllAsync()
    {
      return await _context.Domains.ToListAsync();
    }

    // Get multiple Domains by Ids
    public async Task<IEnumerable<Domain>> GetByIdsAsync(List<Guid> domainIds)
    {
      return await _context.Domains.Where(d => domainIds.Contains(d.Id)).ToListAsync();
    }

    // Add new Domain
    public async Task AddAsync(Domain domain)
    {
      await _context.Domains.AddAsync(domain);
      await _context.SaveChangesAsync();
    }

    // Hard delete domain
    public async Task DeleteAsync(Guid id)
    {
      var domain = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
      _context.Categories.Remove(domain!);
      await _context.SaveChangesAsync();
    }

    // Update domain
    public async Task UpdateAsync(Domain domain)
    {
      _context.Domains.Update(domain);
      await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Guid>> GetDomainIdsByDomainTitles(IEnumerable<string> domainTitles)
    {

      return await _context.Domains
        .Where(d => domainTitles.Contains(d.Title))
        .Select(d => d.Id)
        .ToListAsync();
    }

    public async Task<IEnumerable<Guid>> GetDomainIdsByExamId(Guid examId)
    {
      return await _context.Domains
        .Where(d => d.ExamId == examId)
        .Select(d => d.Id)
        .ToListAsync();
    }

    public async Task<Dictionary<Guid, string>> GetDomainIdsWithTitlesByExamId(Guid examId) {
      return await _context.Domains
        .Where(d => d.ExamId == examId)
        .ToDictionaryAsync(d => d.Id, d => d.Title);
    }


    // Domain Id by Title
    public async Task<Guid> GetIdByTitleAsync(string title, CancellationToken ct)
    {
      return await _context.Domains
          .Where(d => d.Title == title)
          .Select(d => d.Id)
          .FirstOrDefaultAsync(ct);
    }


  }
}
