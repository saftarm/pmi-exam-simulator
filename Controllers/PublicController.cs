using Microsoft.AspNetCore.Mvc;
using TestAPI.Extensions;
using TestAPI.Services.Interfaces;

namespace TestAPI.Controllers;

[ApiController]
public class PublicController(IPublicStatsService publicStatsService) : ControllerBase
{
    private readonly IPublicStatsService _publicStatsService = publicStatsService;

    [HttpGet("/api/public/stats")]
    public async Task<IActionResult> GetStats(CancellationToken ct)
    {
        var result = await _publicStatsService.GetStatsAsync(ct);
        return result.IsSuccess ? Ok(result.Value) : result.ToActionResult();
    }

    [HttpGet("/api/public/settings")]
    public async Task<IActionResult> GetPublicSettings(
        [FromServices] ISiteSettingsService siteSettingsService,
        CancellationToken ct)
    {
        var result = await siteSettingsService.GetAsync(ct);
        if (!result.IsSuccess)
        {
            return result.ToActionResult();
        }

        var s = result.Value!;
        return Ok(new
        {
            s.SiteName,
            s.AllowRegistration,
            s.MaintenanceMode,
        });
    }
}
