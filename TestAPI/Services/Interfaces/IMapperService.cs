using TestAPI.DTO;
using TestAPI.DTO.Exam.Responses;
using TestAPI.Entities;

namespace TestAPI.Services.Interfaces
{
    public interface IMapperService
    {

        // Exams To ExamSummaryDtos
        public ICollection<ExamSummaryDto> MapExamsToExamSummaryDtos(IEnumerable<Exam> exams);

        // Category to CategoryDto
        public CategoryDto MapCategoryToCategoryDto(Category category);

        // Categories to CategoryDtos
        public IEnumerable<CategoryDto> MapCategoriesToCategoryDtos(IEnumerable<Category> categories);
        public ICollection<Exam> MapCreateExamDtosToExam(IEnumerable<CreateExamDto> dto);

        public IEnumerable<CreateExamResponse> MapNewExamsToCreateExamResponse(IEnumerable<Exam> exams);

    }
}
