using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;
using TestAPI.DTO;
using TestAPI.DTO.Category;
using TestAPI.Entities;
using TestAPI.Persistence.Interfaces;
using TestAPI.Services.Interfaces;


namespace TestAPI.Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IExamRepository _examRepository;

        public CategoryService(ICategoryRepository categoryRepository, IExamRepository examRepository)
        {
            _categoryRepository = categoryRepository;
            _examRepository = examRepository;

        }

        private static CategoryDto MapToCategoryDto(Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Title = category.Title,
                ExamSummaryDtos = category.Exams.Select(
                    e => new ExamSummaryDto
                    {
                        Id = e.Id,
                        Title = e.Title,
                        DurationInMinutes = e.DurationInMinutes,
                        NumberOfQuestions = e.NumberOfQuestions
                    }
                )
            };
        }

        //private static CreateCategoryDto MapToCategoryDto(Category category)
        //{
        //    if (category.Exams == null)
        //    {
        //        throw new Exception("Exams not found");
        //    }

        //    return new CreateCategoryDto
        //    {
        //        Id = category.Id,
        //        Title = category.Title,
        //        ExamSummaryDtos = category.Exams.Select(
        //            e => new ExamSummaryDto
        //            {
        //                Id = e.Id,
        //                Title = e.Title,
        //                DurationInMinutes = e.DurationInMinutes,
        //                NumberOfQuestions = e.NumberOfQuestions
        //            }
        //        )
        //    };
        //}



        public async Task<CategoryDto> GetByIdAsync(int categoryId)
        {

            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category == null)
            {
                throw new Exception("Category not Found");
            }

            return MapToCategoryDto(category);


        }

        public async Task<string> GetTitleByExamId(int examId)
        {
            var categoryTitle = await _categoryRepository.GetTitleByExamId(examId);
            return categoryTitle;
        }

        public async Task CreateCategory(CreateCategoryDto createCategoryDto)
        {
            var newCategory = new Category
            {
                Title = createCategoryDto.Title


            };
            await _categoryRepository.AddAsync(newCategory);

        }

        public async Task AddExamToCategory([FromBody] AddExamsToCategoryDto addExamsToCategoryDto)
        {

            var category = await _categoryRepository.GetByIdAsync(addExamsToCategoryDto.CategoryId);

            var exams = await _examRepository.GetAllAsync();


            category.Exams.AddRange(exams.Where(e => addExamsToCategoryDto.ExamIds.Contains(e.Id)));
            await _categoryRepository.Update(category.Id, category);

        }










    }
}
