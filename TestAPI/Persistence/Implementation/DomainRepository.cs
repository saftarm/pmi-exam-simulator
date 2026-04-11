



using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.Entities;
using TestAPI.Exceptions;
using TestAPI.Persistence.Interfaces;

namespace TestAPI.Persistence.Implementation
{


    public class DomainRepository : IDomainRepository
    {

        private readonly ApplicationDbContext _context;

        public DomainRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Domain> GetByIdAsync(Guid id)
        {
            var domain = await _context.Domains.FindAsync(id);
            if (domain == null)
            {
                throw new RecordNotFoundException("Domain Not Found");
            }
            return domain;

        }

        public async Task<IEnumerable<Domain>> GetAllAsync()
        {
            return await _context.Domains.ToListAsync();

        }
        public async Task<IEnumerable<Domain>> GetByIdsAsync(List<Guid> domainIds)
        {
            return await _context.Domains.Where(d => domainIds.Contains(d.Id)).ToListAsync();
        }


        public async Task AddAsync(Domain domain)
        {
            await _context.Domains.AddAsync(domain);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {

            var domain = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (domain == null)
            {
                throw new Exception("Domain Not Found");
            }
            _context.Categories.Remove(domain);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Domain domain)
        {
            _context.Domains.Update(domain);
            await _context.SaveChangesAsync();
        }

        public async Task<Guid> GetIdByTitleAsync(string title, CancellationToken ct)
        {
            return await _context.Domains
                .Where(d => d.Title == title)
                .Select(d => d.Id)
                .FirstOrDefaultAsync(ct);
        }





    }
}