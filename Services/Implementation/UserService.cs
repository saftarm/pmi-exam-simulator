using Microsoft.AspNetCore.Identity;
using TestAPI.DTO.User;
using TestAPI.Entities;
using TestAPI.Enums;
using TestAPI.Models;
using TestAPI.Persistence.Interfaces;
using TestAPI.ResultPattern;
using TestAPI.Services.Interfaces;
using TestAPI.Validation;

namespace TestAPI.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IValidatorResolver _validatorResolver;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(
            IUserRepository userRepository,
            ITokenRepository tokenRepository,
            IPasswordHasher<User> passwordHasher,
            IValidatorResolver validatorResolver,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _passwordHasher = passwordHasher;
            _validatorResolver = validatorResolver;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PagedList<UserListItemDto>>> GetPagedAsync(UserQueryParameters query, CancellationToken ct)
        {
            var page = await _userRepository.GetPagedAsync(query, ct);
            var items = page.Items.Select(MapToListItem).ToList();
            return Result<PagedList<UserListItemDto>>.Success(
                new PagedList<UserListItemDto>(items, page.Page, page.PageSize, page.TotalCount));
        }

        public async Task<Result<UserDto>> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var user = await _userRepository.GetByIdOrDefaultAsync(id);
            if (user == null)
            {
                return Result<UserDto>.Failure(Errors.UserNotFoundById);
            }

            return Result<UserDto>.Success(MapToDto(user));
        }

        public async Task<Result<UserDto>> CreateAsync(CreateUserRequest request, CancellationToken ct)
        {
            var validationResult = await _validatorResolver.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Result<UserDto>.Failure(validationResult.ToError());
            }

            if (!await _userRepository.IsEmailUniqueAsync(request.Email, ct))
            {
                return Result<UserDto>.Failure(Errors.EmailAlreadyExists);
            }

            if (!await _userRepository.IsUserNameUniqueAsync(request.UserName, ct))
            {
                return Result<UserDto>.Failure(Errors.UserNameAlreadyExists);
            }

            var user = new User
            {
                FirstName = request.FirstName,
                UserName = request.UserName,
                DisplayName = request.FirstName,
                Email = request.Email,
                Role = request.Role,
                Status = request.Status
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result<UserDto>.Success(MapToDto(user));
        }

        public async Task<Result> UpdateAsync(Guid id, UpdateUserRequest request, Guid actingUserId, CancellationToken ct)
        {
            var validationResult = await _validatorResolver.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Result.Failure(validationResult.ToError());
            }

            var user = await _userRepository.GetByIdOrDefaultAsync(id);
            if (user == null)
            {
                return Result.Failure(Errors.UserNotFoundById);
            }

            if (!await _userRepository.IsEmailUniqueExceptUserAsync(request.Email, id, ct))
            {
                return Result.Failure(Errors.EmailAlreadyExists);
            }

            if (user.Role == UserRole.Admin && request.Role != UserRole.Admin)
            {
                var adminCount = await _userRepository.CountAdminsAsync(ct);
                if (adminCount <= 1)
                {
                    return Result.Failure(Errors.LastAdminRequired);
                }
            }

            user.UpdateAdminProfile(request.DisplayName, request.Email, request.Role, request.Status);
            await _userRepository.UpdateAsync(user, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            if (request.Status == AccountStatus.Suspended)
            {
                await _tokenRepository.RevokeAllForUserAsync(id, ct);
            }

            return Result.Success();
        }

        public async Task<Result> UpdateStatusAsync(Guid id, UpdateUserStatusRequest request, Guid actingUserId, CancellationToken ct)
        {
            if (id == actingUserId)
            {
                return Result.Failure(Errors.CannotSuspendSelf);
            }

            var user = await _userRepository.GetByIdOrDefaultAsync(id);
            if (user == null)
            {
                return Result.Failure(Errors.UserNotFoundById);
            }

            if (user.Role == UserRole.Admin && request.Status != AccountStatus.Active)
            {
                var adminCount = await _userRepository.CountAdminsAsync(ct);
                if (adminCount <= 1)
                {
                    return Result.Failure(Errors.LastAdminRequired);
                }
            }

            user.SetStatus(request.Status);
            await _userRepository.UpdateAsync(user, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            if (request.Status == AccountStatus.Suspended)
            {
                await _tokenRepository.RevokeAllForUserAsync(id, ct);
            }

            return Result.Success();
        }

        public async Task<Result<int>> GetTotalCountAsync(CancellationToken ct)
        {
            var count = await _userRepository.CountAsync(ct);
            return Result<int>.Success(count);
        }

        public async Task<Result<UserStatsDto>> GetStatsAsync(CancellationToken ct)
        {
            var totalCount = await _userRepository.CountAsync(ct);
            var byRole = await _userRepository.CountByRoleAsync(ct);
            var byStatus = await _userRepository.CountByStatusAsync(ct);

            var stats = new UserStatsDto
            {
                TotalCount = totalCount,
                ByRole = byRole.ToDictionary(
                    kvp => kvp.Key.ToString(),
                    kvp => kvp.Value),
                ByStatus = byStatus.ToDictionary(
                    kvp => kvp.Key.ToString(),
                    kvp => kvp.Value)
            };

            return Result<UserStatsDto>.Success(stats);
        }

        public async Task<Result<UserDto>> UpdateProfileAsync(Guid userId, UpdateProfileRequest request, CancellationToken ct)
        {
            var validationResult = await _validatorResolver.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Result<UserDto>.Failure(validationResult.ToError());
            }

            var user = await _userRepository.GetByIdOrDefaultAsync(userId);
            if (user == null)
            {
                return Result<UserDto>.Failure(Errors.UserNotFoundById);
            }

            if (!await _userRepository.IsEmailUniqueExceptUserAsync(request.Email, userId, ct))
            {
                return Result<UserDto>.Failure(Errors.EmailAlreadyExists);
            }

            user.UpdateProfile(request.DisplayName, request.FirstName, request.Email);
            await _userRepository.UpdateAsync(user, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result<UserDto>.Success(MapToDto(user));
        }

        private static UserDto MapToDto(User user) => new()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            UserName = user.UserName,
            DisplayName = user.DisplayName,
            Email = user.Email,
            Role = user.Role,
            Status = user.Status,
            CreatedAt = user.CreatedAt
        };

        private static UserListItemDto MapToListItem(User user) => new()
        {
            Id = user.Id,
            DisplayName = user.DisplayName,
            Email = user.Email,
            Role = user.Role,
            Status = user.Status,
            CreatedAt = user.CreatedAt,
        };
    }
}
