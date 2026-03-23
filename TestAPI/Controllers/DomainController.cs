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

        public async Task<ActionResult<IEnumerable<DomainDto>>> GetAllDomains()
        {
            return Ok(await _domainService.GetAllAsync());
        }

        [HttpPost("api/domains")]

        public async Task<ActionResult<CategoryDto>> Create(CreateDomainDto dto)
        {
            await _domainService.CreateDomain(dto);
            return Ok();
        }


        [HttpPatch("api/domains/{id}")]

        public async Task<IActionResult> Update(int id, UpdateDomainDto dto) 
        {
            await _domainService.UpdateDomain(id, dto);
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

