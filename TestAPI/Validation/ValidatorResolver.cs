
using FluentValidation;
using FluentValidation.Results;
using TestAPI.DTO;

namespace TestAPI.Validation
{
    public class ValidatorResolver 
    {
      private readonly IServiceProvider _serviceProvider;

        public ValidatorResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<ValidationResult> ValidateAsync(object model){
            var validatorType = typeof(IValidator<>).MakeGenericType(model.GetType());
            var validator = _serviceProvider.GetService(validatorType) as IValidator;

            if(validator == null) {
                  throw new InvalidOperationException($"No validator found for {model.GetType().Name}");
            }
            
            var context = new ValidationContext<object>(model);
            return await validator.ValidateAsync(context);

        }
    }
}
