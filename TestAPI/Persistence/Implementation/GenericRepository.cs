using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.Entities;
using TestAPI.Persistence.Interfaces;


namespace TestAPI.Persistence.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task<IReadOnlyList<T?>> GetAllAsync() {

            return await _context.Set<T>().ToListAsync();
        }
        public async Task<T?> GetByIdAsync(int id) {
            return await _dbSet.FindAsync(id);

        }


        public async Task AddAsync(T entity) {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }



        
        public async Task DeleteAsync (int id) {
            var entity = await _dbSet.FindAsync(id);
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        // Task UpdateAsync(T entity){
            
        // }


        

        


    }
}
