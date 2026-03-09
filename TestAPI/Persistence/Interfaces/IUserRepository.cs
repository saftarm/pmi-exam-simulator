using TestAPI.Entities;

namespace TestAPI.Persistence.Interfaces
{
    public interface IUserRepository
    {


        public Task<User> GetByIdAsync(int userId);

        public Task<User> GetByUserNameAsync(string UserName);
        public Task AddAsync(User user);

        public Task Delete(int userId);




    }
}
