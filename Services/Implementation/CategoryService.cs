using TestAPI.DTO;
using TestAPI.DTO.Category;
using TestAPI.Entities;
using TestAPI.Persistence.Interfaces;
using TestAPI.ResultPattern;
using TestAPI.Services.Interfaces;
using TestAPI.Validation;

namespace TestAPI.Services.Implementation
{
  public class CategoryService : ICategoryService
  {
    private readonly ILogger<CategoryService> _logger;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IValidatorResolver _validatorResolver;

    public CategoryService(
        ILogger<CategoryService> logger,
        ICategoryRepository categoryRepository,
        IValidatorResolver validatorResolver)
    {
      _logger = logger;
      _categoryRepository = categoryRepository;
      _validatorResolver = validatorResolver;
    }

    public async Task<Result> CreateCategory(CreateCategoryDto createCategoryDto, CancellationToken ct)
    {
      var exists = await _categoryRepository.ExistsByTitleAsync(createCategoryDto.Title!, ct);

      if (exists)
      {
        return Result.Failure(Errors.RecordAlreadyExists);
      }

      var newCategory = new Category(
          title: createCategoryDto.Title!,
          description: createCategoryDto.Description!);

      var rowsAffected = await _categoryRepository.AddAsync(newCategory);
      if (_logger.IsEnabled(LogLevel.Information))
      {
        _logger.LogInformation("{RowsAffected} category records are created", rowsAffected);
      }

      return Result.Success();
    }

    public async Task<Result<CategoryDto>> GetByIdAsync(Guid categoryId, CancellationToken ct)
    {
      var category = await _categoryRepository.GetByIdAsync(categoryId);
      if (category == null)
      {
        return Result<CategoryDto>.Failure(Errors.RecordNotFoundById);
      }

      var categoryDto = new CategoryDto
      {
        Id = category.Id,
        Title = category.Title
      };

      return Result<CategoryDto>.Success(categoryDto);
    }

    public async Task<Result<IEnumerable<CategoryDto>>> GetAllAsync(CancellationToken ct)
    {
      var categoriesInDb = await _categoryRepository.GetAllAsync(ct);

      if (!categoriesInDb.Any())
      {
        return Result<IEnumerable<CategoryDto>>.Failure(Errors.RangeOfRecordsNotFound);
      }

      var categoryDtos = categoriesInDb.Select(c => new CategoryDto
      {
        Id = c.Id,
        Title = c.Title,
        Description = c.Description,
        NumberOfExams = c.NumberOfExams
      });

      return Result<IEnumerable<CategoryDto>>.Success(categoryDtos);
    }

    public async Task<Result> UpdateCategory(UpdateCategoryRequest request, CancellationToken ct)
    {

      var validationResult = await _validatorResolver.ValidateAsync(request);

      if (!validationResult.IsValid)
      {
        return Result.Failure(Errors.ValidationFailed);
      }

      var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
      if (category == null)
      {
        return Result.Failure(Errors.RecordNotFoundById);
      }

      category.Title = request.Title;
      category.Description = request.Description;

      await _categoryRepository.Update(category);

      if (_logger.IsEnabled(LogLevel.Information))
      {
        _logger.LogInformation("Category {CategoryId} updated", category.Id);
      }

      return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct)
    {
      var exists = await _categoryRepository.ExistsByIdAsync(id, ct);
      if (!exists)
      {
        return Result.Failure(Errors.RecordNotFoundById);
      }

      await _categoryRepository.DeleteAsync(id);

      if (_logger.IsEnabled(LogLevel.Information))
      {
        _logger.LogInformation("Category {CategoryId} deleted", id);
      }

      return Result.Success();
    }
  }
}
