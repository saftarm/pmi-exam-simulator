using TestAPI.DTO;
using TestAPI.Entities;
using FluentValidation.Results;

namespace TestAPI.Validation
{
      public interface IValidatorResolver
      {
            public Task<ValidationResult> ValidateAsync(object model);
      }
}






