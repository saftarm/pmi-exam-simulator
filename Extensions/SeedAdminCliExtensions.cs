using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TestAPI.Data;
using TestAPI.Entities;
using TestAPI.Enums;
using TestAPI.Models;

namespace TestAPI.Extensions;

public static class SeedAdminCliExtensions
{
    private const string SeedAdminFlag = "--seed-admin";

    public static async Task<bool> TryRunSeedAdminAsync(string[] args)
    {
        if (!args.Contains(SeedAdminFlag))
        {
            return false;
        }

        var builder = Host.CreateApplicationBuilder(args);
        builder.Configuration.AddUserSecrets<Program>(optional: true);
        builder.Services.AddDatabase(builder.Configuration);
        builder.Services.Configure<AdminSeedSettings>(builder.Configuration.GetSection("AdminSeed"));
        builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

        using var host = builder.Build();
        using var scope = host.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();
        var settings = scope.ServiceProvider.GetRequiredService<IOptions<AdminSeedSettings>>().Value;

        if (string.IsNullOrWhiteSpace(settings.UserName)
            || string.IsNullOrWhiteSpace(settings.Password)
            || string.IsNullOrWhiteSpace(settings.Email))
        {
            throw new InvalidOperationException(
                "AdminSeed user secrets are missing. Set AdminSeed:UserName, AdminSeed:Password, and AdminSeed:Email.");
        }

        var admins = await context.Users
            .Where(u => u.Role == UserRole.Admin)
            .ToListAsync();

        if (admins.Count > 0)
        {
            var adminIds = admins.Select(a => a.Id).ToList();
            await context.DomainPerformances
                .Where(dp => adminIds.Contains(dp.UserId))
                .ExecuteDeleteAsync();
            context.Users.RemoveRange(admins);
            await context.SaveChangesAsync();
            Console.WriteLine($"Removed {admins.Count} existing admin account(s).");
        }

        var displayName = string.IsNullOrWhiteSpace(settings.FirstName)
            ? settings.UserName
            : settings.FirstName;

        var admin = new User
        {
            UserName = settings.UserName,
            FirstName = displayName,
            DisplayName = displayName,
            Email = settings.Email,
            Role = UserRole.Admin,
            Status = AccountStatus.Active,
        };
        admin.PasswordHash = passwordHasher.HashPassword(admin, settings.Password);

        await context.Users.AddAsync(admin);
        await context.SaveChangesAsync();

        Console.WriteLine($"Admin user '{settings.UserName}' created.");
        Environment.Exit(0);
        return true;
    }
}
