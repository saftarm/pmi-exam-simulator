using TestAPI.DTO;
using TestAPI.ResultPattern;

namespace TestAPI.Services.Interfaces
{
    public interface IDomainService
    {
        Task<Result<DomainDto>> GetByIdAsync(Guid id);
        Task<Result<IEnumerable<DomainDto>>> GetAllAsync();
        Task<Result<Dictionary<Guid, string>>> GetDomainTitlesByExamIdAsync(Guid examId);
        Task<Result> CreateDomainAsync(CreateDomainDto createDomainDto, CancellationToken ct = default);
        Task<Result> UpdateDomainAsync(Guid id, UpdateDomainDto updateDomainDto, CancellationToken ct = default);
        Task<Result> DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
