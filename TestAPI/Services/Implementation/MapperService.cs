using TestAPI.DTO;
using TestAPI.DTO.Exam.Responses;
using TestAPI.Entities;
using TestAPI.Services.Interfaces;
namespace TestAPI.Services.Implementation
{
    public class MapperService : IMapperService
    {
        public MapperService()
        {

        }
        public ICollection<ExamSummaryDto> MapExamsToExamSummaryDtos(IEnumerable<Exam> exams)
        {
            var examSummaryDtos = exams.Select(e =>
                  new ExamSummaryDto
                  {
                      Title = e.Title,
                      CategoryTitle = e.Category.Title,
                      NumberOfQuestions = e.NumberOfQuestions,
                      DurationInMinutes = e.DurationInMinutes
                  }
            ).ToList();

            return examSummaryDtos;
        }

        public CategoryDto MapCategoryToCategoryDto(Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Title = category.Title
            };
        }


        public IEnumerable<CategoryDto> MapCategoriesToCategoryDtos(IEnumerable<Category> categories)
        {
            return categories.Select(c =>
                new CategoryDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    NumberOfExams = c.NumberOfExams
                }).ToList();
        }

        public ICollection<Exam> MapCreateExamDtosToExam(IEnumerable<CreateExamDto> dto)
        {
            var exams = dto.Select(e => new Exam
            {
                CategoryId = e.CategoryId,
                Title = e.Title,
                Context = e.Context,
                DurationInMinutes = e.DurationInMinutes,
                NumberOfQuestions = e.NumberOfQuestions,
                Domains = e.CreateDomainDtos.Select(
                        d => new Domain
                        {
                            Title = d.Title,
                            Description = d.Description,
                            Weight = d.Weight
                        }
                    ).ToList()
            }).ToList();

            return exams;

        }


        public IEnumerable<CreateExamResponse> MapNewExamsToCreateExamResponse(IEnumerable<Exam> exams)
        {

            var createdExams = exams.Select(e => new CreateExamResponse
            {
                CategoryId = e.CategoryId,
                Title = e.Title,
                Context = e.Context,
                DurationInMinutes = e.DurationInMinutes,
                NumberOfQuestions = e.NumberOfQuestions,
                CreateDomainDtos = e.Domains.Select(d => new CreateDomainDto
                {
                    Title = d.Title,
                    Description = d.Description,
                    Weight = d.Weight
                }).ToList()

            }).ToList();

            return createdExams;
        }


    }
}
