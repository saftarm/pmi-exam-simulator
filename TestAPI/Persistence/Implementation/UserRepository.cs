using TestAPI.Entities;
using TestAPI.Data;
using TestAPI.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TestAPI.Persistence.Implementation


{
    public class UserRepository : IUserRepository
    {

        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

        }

        public async Task Delete(Guid userId) {

            var userInDb = await _context.Users.FindAsync(userId);
            if( userInDb == null ){

                throw new ArgumentNullException($"User with id {userId} not found");
            }
            _context.Users.Remove (userInDb);
            await _context.SaveChangesAsync();

        }

        public async Task<User> GetByIdAsync(Guid userId) {
            var user = await _context.Users.FindAsync(userId);
            if( user == null ){

                throw new ArgumentNullException($"User with id {userId} not found");
            }

            return user;

        }

        public async Task<User> GetByUserNameAsync(string userName) {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if(user == null) {

                throw new ArgumentException($"User not found");
            }

            return user;
        }



    }
}
