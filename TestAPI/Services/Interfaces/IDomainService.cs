using TestAPI.DTO;
using TestAPI.DTO.Category;
using TestAPI.Entities;

namespace TestAPI.Services.Interfaces
{
    public interface IDomainService
    {
        public Task<DomainDto> GetByIdAsync(Guid id);

        public Task<IEnumerable<DomainDto>> GetAllAsync();
    
        public Task CreateDomain(CreateDomainDto createDomainDto);
        
        public Task UpdateDomain(Guid id, UpdateDomainDto updateDomainDto);

        public Task DeleteAsync(Guid id);

    }
}
