using FluentValidation;
using Microsoft.AspNetCore.Identity;
using TestAPI.Entities;
using TestAPI.Persistence.Implementation;
using TestAPI.Persistence.Interfaces;
using TestAPI.Services;
using TestAPI.Services.Implementation;
using TestAPI.Services.Interfaces;
using TestAPI.Validation;

namespace TestAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IExamAttemptService, ExamAttemptService>();
            services.AddScoped<IUserExamResponseRepository, UserExamResponseRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IExamService, ExamService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IDomainService, DomainService>();
            services.AddScoped<IProgressService, ProgressService>();
            services.AddExceptionHandler<ExceptionHandler>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IJWTService, JWTService>();
            services.AddScoped<IValidatorResolver, ValidatorResolver>();
            services.AddSingleton<IMapperService, MapperService>();
            services.AddScoped<IQuestionImportService, QuestionImportService>();
            services.AddValidatorsFromAssemblyContaining<Program>();

            services.Configure<AuthSettings>(configuration.GetSection("AuthSettings"));

            return services;

        }
    }
}
