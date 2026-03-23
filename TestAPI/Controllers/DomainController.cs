using Microsoft.AspNetCore.Mvc;
using TestAPI.DTO;
using TestAPI.DTO.Category;
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



        [HttpGet("/api/domains/{id:int}")]
        public async Task<ActionResult<DomainDto>> GetDomain(int id)
        {
            var category = await _domainService.GetByIdAsync(id);
            return Ok(category);
        }

        [HttpGet("/api/domains")]

        public async Task<ActionResult<IEnumerable<DomainDto>>> GetAllCategories()
        {
            return Ok(await _domainService.GetAllAsync());
        }

        [HttpPost("api/domains")]

        public async Task<ActionResult<CategoryDto>> Create(CreateDomainDto createDomainDto)
        {
            await _domainService.CreateDomain(createDomainDto);
            return Ok();
        }

        [HttpDelete("/api/domains/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _domainService.DeleteAsync(id);
            return NoContent();
        }

       








    }
}

