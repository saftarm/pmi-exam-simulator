using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.Entities;
using TestAPI.Persistence.Interfaces;

namespace TestAPI.Persistence.Implementation;

public class SiteSettingsRepository(ApplicationDbContext context) : ISiteSettingsRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<SiteSettings> GetOrCreateAsync(CancellationToken ct = default)
    {
        var settings = await _context.SiteSettings.FirstOrDefaultAsync(ct);
        if (settings != null)
        {
            return settings;
        }

        settings = new SiteSettings();
        await _context.SiteSettings.AddAsync(settings, ct);
        await _context.SaveChangesAsync(ct);
        return settings;
    }

    public async Task UpdateAsync(SiteSettings settings, CancellationToken ct = default)
    {
        _context.SiteSettings.Update(settings);
        await _context.SaveChangesAsync(ct);
    }
}
