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
            return domains.Select(d =>
                new DomainDto
                {
                    Id = d.Id,
                    Title = d.Title,
                    Description = d.Description,
                    Weight = d.Weight,
                    ExamId = d.ExamId
                
                });
        }

        private static DomainDto MapToDomainDto(Domain domain)
        {

            return new DomainDto
            {
                Id = domain.Id,
                Title = domain.Title,
                Weight = domain.Weight,
                ExamId = domain.ExamId 
            };
        }


        public async Task<DomainDto> GetByIdAsync(int id)
        {

            var domain = await _domainRepository.GetByIdAsync(id);
            if (domain == null) 
            {
                throw new Exception("Category not Found");
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

        public async Task DeleteAsync(int id)
        {
            await _domainRepository.DeleteAsync(id);
        }


        public async Task UpdateDomain(int id, UpdateDomainDto updateDomainDto) {

            var domain = await _domainRepository.GetByIdAsync(id);

            domain.Title = updateDomainDto.Title;
            domain.Description = updateDomainDto.Description;
            domain.Weight = updateDomainDto.Weight;
            await _domainRepository.UpdateAsync(domain);
            

        }

        // public async Task AddExamToCategory([FromBody] AddExamsToCategoryDto addExamsToCategoryDto)
        // {

        //     var category = await _categoryRepository.GetByIdAsync(addExamsToCategoryDto.CategoryId);

        //     var exams = await _examRepository.GetAllAsync();


        //     category.Exams.AddRange(exams.Where(e => addExamsToCategoryDto.ExamIds.Contains(e.Id)));
        //     await _categoryRepository.Update(category.Id, category);

        // }

        //public async Task AddExamsToCategory(AddExamsToCategoryDto addExamsToCategoryDto)
        //{
        //    var category = await _categoryRepository.GetByIdAsync(addExamsToCategoryDto.CategoryId);

        //    var exams = _examRepository.GetByIdAsync(addExamsToCategoryDto.ExamIds);

        //    category.Exams.AddRange(exams);
        //}










    }
}
