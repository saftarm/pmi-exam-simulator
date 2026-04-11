using TestAPI.Persistence.Implementation;
using TestAPI.Persistence.Interfaces;

namespace TestAPI.Extensions
{
    public static class RepositoryExtensions
    {

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IExamRepository, ExamRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IDomainRepository, DomainRepository>();
            services.AddScoped<IExamAttemptRepository, ExamAttemptRepository>();
            return services;


        }
    }
}
