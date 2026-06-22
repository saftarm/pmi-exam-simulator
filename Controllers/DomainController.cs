using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestAPI.DTO;
using TestAPI.DTO.Category;
using TestAPI.Persistence.Implementation;
using TestAPI.Persistence.Interfaces;
using TestAPI.Services.Interfaces;
namespace TestAPI.Controllers
{
  [ApiController]
  public class DomainController : ControllerBase
  {
    private readonly IDomainService _domainService;

    private readonly IDomainRepository _domainRepository;
    public DomainController(IDomainService domainService,
        IDomainRepository domainRepository)
    {
      _domainService = domainService;
      _domainRepository = domainRepository;
    }
    [HttpGet("/api/domains/withTitles")]
    public async Task<ActionResult<Dictionary<Guid, string>>> GetDomainWithItsTitlesByExamId(Guid examId)
    {
      var domainIds = await _domainRepository.GetDomainIdsWithTitlesByExamId(examId);
      return Ok(domainIds);
    }


    // Get Domain By Id 
    [HttpGet("/api/domains/{id}")]
    public async Task<ActionResult<DomainDto>> GetDomain(Guid id)
    {
      var category = await _domainService.GetByIdAsync(id);
      return Ok(category);
    }

    // Get All Domains
    [HttpGet("/api/domains")]

    public async Task<ActionResult<IEnumerable<DomainDto>>> GetAllDomains()
    {
      return Ok(await _domainService.GetAllAsync());
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost("/api/domains")]

    public async Task<ActionResult<CategoryDto>> Create(CreateDomainDto dto)
    {
      await _domainService.CreateDomain(dto);
      return Ok();
    }

    // Update Domain
    [Authorize(Policy = "AdminOnly")]
    [HttpPut("/api/domains/{id}")]

    public async Task<IActionResult> Update(Guid id, UpdateDomainDto dto)
    {
      await _domainService.UpdateDomain(id, dto);
      return Ok();
    }

    // Delete Domain
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("/api/domains/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
      await _domainService.DeleteAsync(id);
      return NoContent();
    }



  }
}

