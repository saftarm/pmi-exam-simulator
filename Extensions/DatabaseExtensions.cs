using Microsoft.EntityFrameworkCore;
using TestAPI.Data;

namespace TestAPI.Extensions
{
  public static class DatabaseExtensions
  {
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
      var connectionString = configuration.GetConnectionString("PostgreSqlConnection");
      services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
      return services;
    }
  }
}
