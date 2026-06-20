using System.Text.Json.Serialization;
using TestAPI.Enums;

namespace TestAPI.DTO.User
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public AccountStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
