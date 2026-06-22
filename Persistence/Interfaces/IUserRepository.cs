using TestAPI.DTO.User;
using TestAPI.Entities;
using TestAPI.Enums;
using TestAPI.Models;
using TestAPI.ResultPattern;

namespace TestAPI.Persistence.Interfaces
{
  public interface IUserRepository
  {
    public Task<bool> UserExistsByUserId(Guid userId);

    public Task<User?> GetByIdOrDefaultAsync(Guid userId);
    public Task<User?> GetByUserNameAsync(string userName);
    public Task AddAsync(User user);
    public Task<bool> DeleteAsync(Guid userId);
    public Task<bool> IsEmailUniqueAsync(string email, CancellationToken ct);
    public Task<bool> IsUserNameUniqueAsync(string userName, CancellationToken ct);
    public Task<bool> IsEmailUniqueExceptUserAsync(string email, Guid userId, CancellationToken ct);
    public Task<PagedList<User>> GetPagedAsync(UserQueryParameters query, CancellationToken ct);
    public Task<int> UpdateAsync(User user, CancellationToken ct);
    public Task<int> CountAsync(CancellationToken ct);
    public Task<int> CountAdminsAsync(CancellationToken ct);
    Task<Dictionary<UserRole, int>> CountByRoleAsync(CancellationToken ct);
    Task<Dictionary<AccountStatus, int>> CountByStatusAsync(CancellationToken ct);
  }
}
