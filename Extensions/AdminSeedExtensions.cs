using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestAPI.Data;
using TestAPI.Entities;
using TestAPI.Enums;

namespace TestAPI.Extensions
{
    public static class AdminSeedExtensions
    {
        public static async Task SeedAdminUserAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();

            if (await context.Users.AnyAsync(u => u.Role == UserRole.Admin))
            {
                return;
            }

            await EnsureAdminUserAsync(context, passwordHasher, "admin", "Admin123!", "Admin", "admin@pmi.local");
        }

        public static async Task EnsureSaftarAdminAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();

            await EnsureAdminUserAsync(context, passwordHasher, "saftar", "saftarMSN1776@", "Saftar", "saftar@pmi.local");
        }

        public static async Task RunDatabaseSeedAsync(string[] args)
        {
            if (!args.Contains("--seed-users"))
            {
                return;
            }

            var builder = Host.CreateApplicationBuilder(args);
            builder.Configuration.AddUserSecrets<Program>(optional: true);
            builder.Services.AddDatabase(builder.Configuration);
            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            using var host = builder.Build();
            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();

            await EnsureAdminUserAsync(context, passwordHasher, "saftar", "saftarMSN1776@", "Saftar", "saftar@pmi.local");
            Console.WriteLine("Admin user 'saftar' ensured in database.");
            Environment.Exit(0);
        }

        private static async Task EnsureAdminUserAsync(
            ApplicationDbContext context,
            IPasswordHasher<User> passwordHasher,
            string userName,
            string password,
            string firstName,
            string email)
        {
            var existing = await context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if (existing != null)
            {
                existing.FirstName = firstName;
                existing.DisplayName = firstName;
                existing.Email = email;
                existing.Role = UserRole.Admin;
                existing.Status = AccountStatus.Active;
                existing.PasswordHash = passwordHasher.HashPassword(existing, password);
                context.Users.Update(existing);
            }
            else
            {
                var user = new User
                {
                    FirstName = firstName,
                    UserName = userName,
                    DisplayName = firstName,
                    Email = email,
                    Role = UserRole.Admin,
                    Status = AccountStatus.Active,
                };
                user.PasswordHash = passwordHasher.HashPassword(user, password);
                await context.Users.AddAsync(user);
            }

            await context.SaveChangesAsync();
        }
    }
}
