using TestAPI.Entities;

namespace TestAPI.Persistence.Interfaces
{
  public interface IDomainRepository
  {

    public Task<IEnumerable<Domain>> GetDomainsByExamIdAsync(Guid examId);

    public Task<IEnumerable<Guid>> GetDomainIdsByDomainTitles(IEnumerable<string> domainTitles);


    public Task<IEnumerable<Guid>> GetDomainIdsByExamId(Guid examId);
    public Task<Dictionary<Guid, string>> GetDomainIdsWithTitlesByExamId(Guid examId);



    public Task<Domain?> GetByIdAsync(Guid id);
    public Task<IEnumerable<Domain>> GetAllAsync();
    public Task<IEnumerable<Domain>> GetByIdsAsync(List<Guid> domainIds);
    public Task AddAsync(Domain domain);
    public Task UpdateAsync(Domain domain);
    public Task DeleteAsync(Guid id);
    public Task<Guid> GetIdByTitleAsync(string title, CancellationToken ct);
  }
}
