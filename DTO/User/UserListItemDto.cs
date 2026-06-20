using TestAPI.Enums;

namespace TestAPI.DTO.User
{
    public class UserListItemDto
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public AccountStatus Status { get; set; }
    }
}
