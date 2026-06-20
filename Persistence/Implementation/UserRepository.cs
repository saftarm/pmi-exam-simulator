using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.DTO.User;
using TestAPI.Entities;
using TestAPI.Enums;
using TestAPI.Models;
using TestAPI.Persistence.Interfaces;

namespace TestAPI.Persistence.Implementation


{
  public class UserRepository(ApplicationDbContext context) : IUserRepository
  {
    private readonly ApplicationDbContext _context = context;

    public async Task<bool> UserExistsByUserId(Guid userId)
    {
      return await _context.Users.AnyAsync(u => u.Id == userId);
    }

    public async Task AddAsync(User user)
    {
      await _context.Users.AddAsync(user);
      await _context.SaveChangesAsync();
    }

    public async Task Delete(Guid userId)
    {
      var userInDb = await _context.Users.FindAsync(userId);
      if (userInDb == null)
      {
        throw new ArgumentNullException($"User with id {userId} not found");
      }
      _context.Users.Remove(userInDb);
      await _context.SaveChangesAsync();
    }

    public async Task<User?> GetByIdOrDefaultAsync(Guid userId)
    {
      return await _context.Users.FindAsync(userId);
    }

    public async Task<User?> GetByUserNameAsync(string userName)
    {
      return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
    }

    public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken ct)
    {
      return !await _context.Users.AnyAsync(u => u.Email == email, ct);
    }

    public async Task<bool> IsUserNameUniqueAsync(string userName, CancellationToken ct)
    {
      return !await _context.Users.AnyAsync(u => u.UserName == userName, ct);
    }

    public async Task<bool> IsEmailUniqueExceptUserAsync(string email, Guid userId, CancellationToken ct)
    {
      return !await _context.Users.AnyAsync(u => u.Email == email && u.Id != userId, ct);
    }

    public async Task<PagedList<User>> GetPagedAsync(UserQueryParameters query, CancellationToken ct)
    {
      var page = query.PageNumber > 0 ? query.PageNumber : 1;
      var pageSize = query.PageSize > 0 ? query.PageSize : 20;

      var usersQuery = _context.Users.AsNoTracking().AsQueryable();

      if (!string.IsNullOrWhiteSpace(query.Search))
      {
        var term = query.Search.Trim();
        usersQuery = usersQuery.Where(u =>
            u.FirstName.Contains(term) ||
            u.UserName.Contains(term) ||
            u.DisplayName.Contains(term) ||
            u.Email.Contains(term));
      }

      if (query.Role.HasValue)
      {
        usersQuery = usersQuery.Where(u => u.Role == query.Role.Value);
      }

      if (query.Status.HasValue)
      {
        usersQuery = usersQuery.Where(u => u.Status == query.Status.Value);
      }

      usersQuery = usersQuery.OrderByDescending(u => u.CreatedAt);

      return await PagedList<User>.CreateAsync(usersQuery, page, pageSize);
    }

    public async Task<int> UpdateAsync(User user, CancellationToken ct)
    {
      _context.Users.Update(user);
      return await _context.SaveChangesAsync(ct);
    }

    public async Task<int> CountAsync(CancellationToken ct)
    {
      return await _context.Users.CountAsync(ct);
    }

    public async Task<int> CountAdminsAsync(CancellationToken ct)
    {
      return await _context.Users.CountAsync(u => u.Role == UserRole.Admin, ct);
    }

    public async Task<Dictionary<UserRole, int>> CountByRoleAsync(CancellationToken ct)
    {
      var counts = await _context.Users
          .AsNoTracking()
          .GroupBy(u => u.Role)
          .Select(g => new { Role = g.Key, Count = g.Count() })
          .ToListAsync(ct);

      return counts.ToDictionary(x => x.Role, x => x.Count);
    }

    public async Task<Dictionary<AccountStatus, int>> CountByStatusAsync(CancellationToken ct)
    {
      var counts = await _context.Users
          .AsNoTracking()
          .GroupBy(u => u.Status)
          .Select(g => new { Status = g.Key, Count = g.Count() })
          .ToListAsync(ct);

      return counts.ToDictionary(x => x.Status, x => x.Count);
    }
  }
}
