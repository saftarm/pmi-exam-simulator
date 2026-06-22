using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestAPI.DTO.Settings;
using TestAPI.Extensions;
using TestAPI.Services.Interfaces;

namespace TestAPI.Controllers;

[ApiController]
[Authorize(Policy = "AdminOnly")]
public class AdminSettingsController(ISiteSettingsService siteSettingsService) : ControllerBase
{
    private readonly ISiteSettingsService _siteSettingsService = siteSettingsService;

    [HttpGet("/api/admin/settings")]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        var result = await _siteSettingsService.GetAsync(ct);
        return result.IsSuccess ? Ok(result.Value) : result.ToActionResult();
    }

    [HttpPut("/api/admin/settings")]
    public async Task<IActionResult> Update([FromBody] UpdateSiteSettingsDto dto, CancellationToken ct)
    {
        var result = await _siteSettingsService.UpdateAsync(dto, ct);
        return result.IsSuccess ? Ok(result.Value) : result.ToActionResult();
    }
}
