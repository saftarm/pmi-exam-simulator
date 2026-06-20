using TestAPI.DTO.Question;
using TestAPI.DTO.User;
using TestAPI.Models;
using TestAPI.ResultPattern;

namespace TestAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<Result<PagedList<UserListItemDto>>> GetPagedAsync(UserQueryParameters query, CancellationToken ct);
        Task<Result<UserDto>> GetByIdAsync(Guid id, CancellationToken ct);
        Task<Result<UserDto>> CreateAsync(CreateUserRequest request, CancellationToken ct);
        Task<Result> UpdateAsync(Guid id, UpdateUserRequest request, Guid actingUserId, CancellationToken ct);
        Task<Result> UpdateStatusAsync(Guid id, UpdateUserStatusRequest request, Guid actingUserId, CancellationToken ct);
        Task<Result<int>> GetTotalCountAsync(CancellationToken ct);
        Task<Result<UserStatsDto>> GetStatsAsync(CancellationToken ct);
    }
}
