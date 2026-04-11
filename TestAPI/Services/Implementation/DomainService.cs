using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;
using TestAPI.DTO;
using TestAPI.DTO.Category;
using TestAPI.Entities;
using TestAPI.Persistence.Interfaces;
using TestAPI.Services.Interfaces;


namespace TestAPI.Services.Implementation
{
    public class DomainService : IDomainService
    {
        private readonly IDomainRepository _domainRepository;
        private readonly IExamRepository _examRepository;

        public DomainService(IDomainRepository domainRepository, IExamRepository examRepository)
        {
            _domainRepository = domainRepository;
            _examRepository = examRepository;

        }



        // private static IEnumerable<ExamSummaryDto> MapToExamSummaryDtos(IEnumerable<Exam> exams)
        // {
        //     return exams.Select(e =>
        //     new ExamSummaryDto
        //     {
        //         Id = e.Id,
        //         Title = e.Title,
        //         DurationInMinutes = e.DurationInMinutes,
        //         NumberOfQuestions = e.NumberOfQuestions
        //     });
        // }

        private static IEnumerable<DomainDto> MapToDomainDtos(IEnumerable<Domain> domains)
        {
            return domains.Select(dto =>
                new DomainDto
                {
                    Id = dto.Id,
                    Title = dto.Title,
                    Description = dto.Description,
                    Weight = dto.Weight,
                    ExamId = dto.ExamId
                
                });
        }

        private static DomainDto MapToDomainDto(Domain domain)
        {
            return new DomainDto
            {
                Id = domain.Id,
                Title = domain.Title,
                Description = domain.Description,
                Weight = domain.Weight,
                ExamId = domain.ExamId 
            };
        } 

        public async Task<DomainDto> GetByIdAsync(Guid id)
        {

            var domain = await _domainRepository.GetByIdAsync(id);
            if (domain == null) 
            {
                throw new Exception("Domain by Id not Found");
            }

            return MapToDomainDto(domain);

        }

        public async Task<IEnumerable<DomainDto>> GetAllAsync()
        {
            var domains = await _domainRepository.GetAllAsync();
       
            return MapToDomainDtos(domains);
        }

   
        public async Task CreateDomain(CreateDomainDto createDomainDto)
        {


            var newDomain = new Domain
            {
                Title = createDomainDto.Title,
                Description = createDomainDto.Description,
                Weight = createDomainDto.Weight
                

            };
            await _domainRepository.AddAsync(newDomain);

        }

        public async Task DeleteAsync(Guid id)
        {
            await _domainRepository.DeleteAsync(id);
        }


        public async Task UpdateDomain(Guid id, UpdateDomainDto updateDomainDto) {

            var domain = await _domainRepository.GetByIdAsync(id);

            domain.Title = updateDomainDto.Title;
            domain.Description = updateDomainDto.Description;
            domain.Weight = updateDomainDto.Weight;
            await _domainRepository.UpdateAsync(domain);
            

        }

    }
}
