using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestAPI.Extensions;
using TestAPI.Services.Interfaces;

namespace TestAPI.Controllers;

[ApiController]
[Authorize(Policy = "AdminOnly")]
public class AdminAnalyticsController(IAnalyticsService analyticsService) : ControllerBase
{
    private readonly IAnalyticsService _analyticsService = analyticsService;

    [HttpGet("/api/admin/analytics/attempts")]
    public async Task<IActionResult> GetAttemptVolume([FromQuery] int days = 30, CancellationToken ct = default)
    {
        var result = await _analyticsService.GetAttemptVolumeAsync(days, ct);
        return result.IsSuccess ? Ok(result.Value) : result.ToActionResult();
    }

    [HttpGet("/api/admin/analytics/pass-rate")]
    public async Task<IActionResult> GetPassRate(CancellationToken ct = default)
    {
        var result = await _analyticsService.GetPassRateAsync(ct);
        return result.IsSuccess ? Ok(result.Value) : result.ToActionResult();
    }
}
