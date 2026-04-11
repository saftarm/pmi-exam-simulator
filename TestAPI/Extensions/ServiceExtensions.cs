using FluentValidation;
using Microsoft.AspNetCore.Identity;
using TestAPI.Entities;
using TestAPI.Services;
using TestAPI.Services.Implementation;
using TestAPI.Services.Interfaces;

namespace TestAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IExamAttemptService, ExamAttemptService>();
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

            services.AddSingleton<IMapperService, MapperService>();
            services.AddScoped<IQuestionImportService, QuestionImportService>();
            services.AddValidatorsFromAssemblyContaining<Program>();



            return services;

        }
    }
}
