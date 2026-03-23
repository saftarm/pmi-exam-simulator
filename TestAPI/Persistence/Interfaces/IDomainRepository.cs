using TestAPI.DTO;
using TestAPI.Entities;

namespace TestAPI.Persistence.Interfaces
{
    public interface IDomainRepository
    {
        public Task<Domain> GetByIdAsync(int id);
        public Task<IEnumerable<Domain>> GetAllAsync();

        public Task<IEnumerable<Domain>> GetByIdsAsync(List<int> domainIds);

        public Task AddAsync(Domain domain);
        public Task UpdateAsync(Domain domain);

        public Task DeleteAsync(int id); 


    }
}