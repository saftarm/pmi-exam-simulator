using TestAPI.Entities;

namespace TestAPI.Persistence.Interfaces
{
    public interface IDomainRepository
    {
        public Task<Domain> GetByIdAsync(Guid id);
        public Task<IEnumerable<Domain>> GetAllAsync();

        public Task<IEnumerable<Domain>> GetByIdsAsync(List<Guid> domainIds);

        public Task AddAsync(Domain domain);
        public Task UpdateAsync(Domain domain);

        public Task DeleteAsync(Guid id);

        public Task<Guid> GetIdByTitleAsync(string title, CancellationToken ct);


    }
}