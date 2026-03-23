using Microsoft.AspNetCore.Http.HttpResults;
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



        private static IEnumerable<ExamSummaryDto> MapToExamSummaryDtos(IEnumerable<Exam> exams)
        {
            return exams.Select(e =>
            new ExamSummaryDto
            {
                Id = e.Id,
                Title = e.Title,
                DurationInMinutes = e.DurationInMinutes,
                NumberOfQuestions = e.NumberOfQuestions
            });
        }

        private static IEnumerable<CategoryDto> MapToCategoryDtos(IEnumerable<Category> categories)
        {
            return categories.Select(c =>
                new CategoryDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    NumberOfExams = c.NumberOfExams
                });
        }

        private static CategoryDto MapToCategoryDto(Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Title = category.Title   
            };
        }

        public async Task<IEnumerable<ExamSummaryDto>> GetExamSummariesByCategoryId(int categoryId)
        {
            var exams = await  _categoryRepository.GetExamsByCategoryId(categoryId);

            return MapToExamSummaryDtos(exams);

        }

        public async Task<CategoryDto> GetByIdAsync(int categoryId)
        {
           
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category == null)
            {
                throw new Exception("Category not Found");
            }

            return MapToCategoryDto(category);

        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
       
            return MapToCategoryDtos(categories);
        }

   
        public async Task CreateCategory(CreateCategoryDto createCategoryDto)
        {
            var newCategory = new Category
            {
                Title = createCategoryDto.Title,
                Description = createCategoryDto.Description

            };
            await _categoryRepository.AddAsync(newCategory);

        }

         public async Task UpdateCategory(int id, UpdateCategoryDto dto) {

            var category = await _categoryRepository.GetByIdAsync(id);

            if(category == null) {
                throw new KeyNotFoundException();
            }

            
            if(dto.Title != null) {
                category.Title = dto.Title;
            }
            if(dto.Description != null ){
                category.Description = dto.Description;
            }
            
            await _categoryRepository.Update(category);

         }

        public async Task DeleteAsync(int id)
        {
            await _categoryRepository.DeleteAsync(id);
        }

        public async Task AddExamsToCategory(AddExamsToCategoryDto addExamsToCategoryDto)
        {
            var category = await _categoryRepository.GetByIdAsync(addExamsToCategoryDto.CategoryId);

            var existingIds = category.Exams.Select(e => e.Id);

            var newExamIds = addExamsToCategoryDto.ExamIds
            .Where(id => !existingIds.Contains(id))
            .ToList();

            var exams = await _examRepository.GetAllById(newExamIds);

            if(!newExamIds.Any()) {
                return;
            }

            category.Exams.AddRange(exams);
            category.NumberOfExams += newExamIds.Count();
            await _categoryRepository.Update(category);

        }


    }
}
