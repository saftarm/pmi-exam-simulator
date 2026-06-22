using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.DTO.Public;
using TestAPI.Enums;
using TestAPI.ResultPattern;
using TestAPI.Services.Interfaces;

namespace TestAPI.Services.Implementation;

public class PublicStatsService(ApplicationDbContext context) : IPublicStatsService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<PublicStatsDto>> GetStatsAsync(CancellationToken ct = default)
    {
        var totalQuestions = await _context.Questions.CountAsync(ct);
        var totalUsers = await _context.Users.CountAsync(ct);
        var publishedExamCount = await _context.Exams
            .CountAsync(e => e.Status == ExamStatus.Published, ct);

        return Result<PublicStatsDto>.Success(new PublicStatsDto
        {
            TotalQuestions = totalQuestions,
            TotalUsers = totalUsers,
            PublishedExamCount = publishedExamCount,
        });
    }
}
