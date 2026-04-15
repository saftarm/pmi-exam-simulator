using TestAPI.Data;
using TestAPI.Entities;
using TestAPI.Persistence.Interfaces;

namespace TestAPI.Persistence.Implementation
{
    public class UserExamResponseRepository : IUserExamResponseRepository
    {
        private readonly ApplicationDbContext _context;
        public UserExamResponseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> AddAsync(UserExamResponse response)
        {
            await _context.UserExamResponses.AddAsync(response);
            await _context.SaveChangesAsync();
            return response.Id;
        }
    }
}
