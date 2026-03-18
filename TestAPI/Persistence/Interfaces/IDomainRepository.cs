using TestAPI.DTO;
using TestAPI.Entities;

namespace TestAPI.Persistence.Interfaces
{
    public interface IDomainRepository
    {
        public Task<Domain> GetByIdAsync(int categoryId);
        public Task<IEnumerable<Domain>> GetAllAsync();

        public Task AddAsync(Domain domain);
        public Task UpdateAsync(Domain domain);

        public Task DeleteAsync(int id); 


    }
}