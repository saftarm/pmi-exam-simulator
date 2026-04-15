using TestAPI.Entities;

namespace TestAPI.Persistence.Interfaces
{
    public interface IUserRepository
    {
        public Task<User> GetByIdAsync(Guid userId);
        public Task<User> GetByUserNameAsync(string UserName);
        public Task AddAsync(User user);
        public Task Delete(Guid userId);
        public Task<bool> IsEmailUniqueAsync(string email, CancellationToken ct);
        public Task<bool> IsUserNameUniqueAsync(string userName, CancellationToken ct);

    }
}
