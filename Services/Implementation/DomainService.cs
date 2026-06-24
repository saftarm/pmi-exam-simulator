using TestAPI.DTO;
using TestAPI.Entities;
using TestAPI.Persistence.Interfaces;
using TestAPI.ResultPattern;
using TestAPI.Services.Interfaces;
using TestAPI.Validation;

namespace TestAPI.Services.Implementation
{
    public class DomainService : IDomainService
    {
        private readonly IDomainRepository _domainRepository;
        private readonly IExamRepository _examRepository;
        private readonly IValidatorResolver _validatorResolver;
        private readonly IUnitOfWork _unitOfWork;

        public DomainService(
            IDomainRepository domainRepository,
            IExamRepository examRepository,
            IValidatorResolver validatorResolver,
            IUnitOfWork unitOfWork)
        {
            _domainRepository = domainRepository;
            _examRepository = examRepository;
            _validatorResolver = validatorResolver;
            _unitOfWork = unitOfWork;
        }

        private static IEnumerable<DomainDto> MapToDomainDtos(IEnumerable<Domain> domains)
        {
            return domains.Select(MapToDomainDto);
        }

        private static DomainDto MapToDomainDto(Domain domain)
        {
            return new DomainDto
            {
                Id = domain.Id,
                Title = domain.Title,
                Description = domain.Description,
                Weight = domain.Weight,
                ExamId = domain.ExamId,
            };
        }

        public async Task<Result<DomainDto>> GetByIdAsync(Guid id)
        {
            var domain = await _domainRepository.GetByIdAsync(id);
            if (domain == null)
            {
                return Result<DomainDto>.Failure(Errors.DomainNotFound);
            }

            return Result<DomainDto>.Success(MapToDomainDto(domain));
        }

        public async Task<Result<IEnumerable<DomainDto>>> GetAllAsync()
        {
            var domains = await _domainRepository.GetAllAsync();
            return Result<IEnumerable<DomainDto>>.Success(MapToDomainDtos(domains));
        }

        public async Task<Result<Dictionary<Guid, string>>> GetDomainTitlesByExamIdAsync(Guid examId)
        {
            var exam = await _examRepository.GetByIdAsync(examId);
            if (exam == null)
            {
                return Result<Dictionary<Guid, string>>.Failure(Errors.ExamNotFound);
            }

            var domainIds = await _domainRepository.GetDomainIdsWithTitlesByExamId(examId);
            return Result<Dictionary<Guid, string>>.Success(domainIds);
        }

        public async Task<Result> CreateDomainAsync(CreateDomainDto createDomainDto, CancellationToken ct = default)
        {
            var validationResult = await _validatorResolver.ValidateAsync(createDomainDto);
            if (!validationResult.IsValid)
            {
                return Result.Failure(validationResult.ToError());
            }

            var exam = await _examRepository.GetByIdAsync(createDomainDto.ExamId);
            if (exam == null)
            {
                return Result.Failure(Errors.ExamNotFound);
            }

            var newDomain = new Domain(
                createDomainDto.Title,
                createDomainDto.Description,
                createDomainDto.Weight,
                createDomainDto.ExamId);

            await _domainRepository.AddAsync(newDomain);
            await _unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }

        public async Task<Result> UpdateDomainAsync(
            Guid id,
            UpdateDomainDto updateDomainDto,
            CancellationToken ct = default)
        {
            var validationResult = await _validatorResolver.ValidateAsync(updateDomainDto);
            if (!validationResult.IsValid)
            {
                return Result.Failure(validationResult.ToError());
            }

            var domain = await _domainRepository.GetByIdAsync(id);
            if (domain == null)
            {
                return Result.Failure(Errors.DomainNotFound);
            }

            domain.Update(updateDomainDto.Title, updateDomainDto.Description, updateDomainDto.Weight);
            await _domainRepository.UpdateAsync(domain);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }

        public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var domain = await _domainRepository.GetByIdAsync(id);
            if (domain == null)
            {
                return Result.Failure(Errors.DomainNotFound);
            }

            await _domainRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
