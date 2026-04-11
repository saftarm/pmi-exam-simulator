using NuGet.Packaging;
using TestAPI.DTO;
using TestAPI.DTO.Category;
using TestAPI.Entities;
using TestAPI.Exceptions;
using TestAPI.Persistence.Interfaces;
using TestAPI.Services.Interfaces;


namespace TestAPI.Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IExamRepository _examRepository;
        private readonly IMapperService _mapperService;

        public CategoryService(ICategoryRepository categoryRepository, IExamRepository examRepository, IMapperService mapperService)
        {
            _categoryRepository = categoryRepository;
            _examRepository = examRepository;
            _mapperService = mapperService;
        }

        public async Task<CategoryDto> CreateCategory(CreateCategoryDto createCategoryDto, CancellationToken ct)
        {

            var exists = await _categoryRepository.ExistsByTitleAsync(createCategoryDto.Title, ct);

            if (exists)
            {
                throw new RecordAlreadyExists($"Category with name {createCategoryDto.Title} already exists");
            }

            var newCategory = new Category
            {
                Title = createCategoryDto.Title,
                Description = createCategoryDto.Description
            };

            await _categoryRepository.AddAsync(newCategory);

            return new CategoryDto
            {
                Id = newCategory.Id,
                Title = newCategory.Title,
                Description = newCategory.Description,
                NumberOfExams = newCategory.NumberOfExams
            };

        }
        // Get Category By Id
        public async Task<CategoryDto> GetByIdAsync(Guid categoryId, CancellationToken ct)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category == null)
            {
                throw new RecordNotFoundException($"Category not Found by id = {categoryId}");
            }
            return _mapperService.MapCategoryToCategoryDto(category);
        }



        public async Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken ct)
        {
            var categories = await _categoryRepository.GetAllAsync(ct);

            if (categories == null)
            {
                throw new RecordNotFoundException("Categories not found");
            }

            return _mapperService.MapCategoriesToCategoryDtos(categories);
        }


        // Update Category
        public async Task UpdateCategory(Guid id, UpdateCategoryDto dto)
        {

            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                throw new KeyNotFoundException();
            }


            if (dto.Title != null)
            {
                category.Title = dto.Title;
            }
            if (dto.Description != null)
            {
                category.Description = dto.Description;
            }

            await _categoryRepository.Update(category);

        }

        public async Task DeleteAsync(Guid id)
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

            if (!newExamIds.Any())
            {
                return;
            }

            category.Exams.AddRange(exams);
            category.NumberOfExams += newExamIds.Count();
            await _categoryRepository.Update(category);

        }


    }
}
