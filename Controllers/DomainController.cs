using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestAPI.DTO;
using TestAPI.Extensions;
using TestAPI.Services.Interfaces;

namespace TestAPI.Controllers
{
  [ApiController]
  public class DomainController : ControllerBase
  {
    private readonly IDomainService _domainService;

    public DomainController(IDomainService domainService)
    {
      _domainService = domainService;
    }

    [HttpGet("/api/domains/withTitles")]
    public async Task<IActionResult> GetDomainWithItsTitlesByExamId(Guid examId)
    {
      var result = await _domainService.GetDomainTitlesByExamIdAsync(examId);
      return result.IsSuccess ? Ok(result.Value) : result.ToActionResult();
    }

    [HttpGet("/api/domains/{id}")]
    public async Task<IActionResult> GetDomain(Guid id)
    {
      var result = await _domainService.GetByIdAsync(id);
      return result.IsSuccess ? Ok(result.Value) : result.ToActionResult();
    }

    [HttpGet("/api/domains")]
    public async Task<IActionResult> GetAllDomains()
    {
      var result = await _domainService.GetAllAsync();
      return result.IsSuccess ? Ok(result.Value) : result.ToActionResult();
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost("/api/domains")]
    public async Task<IActionResult> Create(CreateDomainDto dto, CancellationToken ct)
    {
      var result = await _domainService.CreateDomainAsync(dto, ct);
      return result.IsSuccess ? Ok() : result.ToActionResult();
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPut("/api/domains/{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateDomainDto dto, CancellationToken ct)
    {
      var result = await _domainService.UpdateDomainAsync(id, dto, ct);
      return result.IsSuccess ? NoContent() : result.ToActionResult();
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("/api/domains/{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
      var result = await _domainService.DeleteAsync(id, ct);
      return result.ToActionResult();
    }
  }
}
