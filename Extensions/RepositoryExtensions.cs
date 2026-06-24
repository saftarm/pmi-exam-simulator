using TestAPI.Persistence.Implementation;
using TestAPI.Persistence.Interfaces;

namespace TestAPI.Extensions
{
    public static class RepositoryExtensions
    {

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IExamRepository, ExamRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IDomainRepository, DomainRepository>();
            services.AddScoped<IExamAttemptRepository, ExamAttemptRepository>();
            services.AddScoped<IDomainPerformanceRepository, DomainPerformanceRepository>();
            services.AddScoped<IPublicStatsRepository, PublicStatsRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<ISiteSettingsRepository, SiteSettingsRepository>();
            return services;


        }
    }
}
