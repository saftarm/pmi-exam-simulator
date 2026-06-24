using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.DTO.Public;
using TestAPI.Enums;
using TestAPI.Persistence.Interfaces;

namespace TestAPI.Persistence.Implementation;

public class PublicStatsRepository(ApplicationDbContext context) : IPublicStatsRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<PublicStatsDto> GetStatsAsync(CancellationToken ct = default)
    {
        var totalQuestions = await _context.Questions.CountAsync(ct);
        var totalUsers = await _context.Users.CountAsync(ct);
        var publishedExamCount = await _context.Exams
            .CountAsync(e => e.Status == ExamStatus.Published, ct);

        return new PublicStatsDto
        {
            TotalQuestions = totalQuestions,
            TotalUsers = totalUsers,
            PublishedExamCount = publishedExamCount,
        };
    }
}
