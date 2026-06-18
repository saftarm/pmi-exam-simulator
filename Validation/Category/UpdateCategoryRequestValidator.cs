
using DocumentFormat.OpenXml.InkML;
using FluentValidation;
using TestAPI.DTO;
using TestAPI.Persistence.Interfaces;

namespace TestAPI.Validation
{
    public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
    {
        private readonly ICategoryRepository _categoryRepository;

        public UpdateCategoryRequestValidator(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            
            RuleFor(r => r.CategoryId)
            .MustAsync(async (categoryId, ct) => 
                   await _categoryRepository.ExistsByIdAsync(categoryId,ct))
                  .WithMessage("Category with given Id not found");

            RuleFor(r => r.Title)
            .NotNull().NotEmpty().WithMessage("Category Title is required")
            .MaximumLength(200).WithMessage("Category Title must not contain more than 200 characters");

            RuleFor(r => r.Title)
            .NotNull().NotEmpty().WithMessage("Category Description is required")
            .MaximumLength(200).WithMessage("Category Description must not contain more than 200 characters");

        }
    }
}
