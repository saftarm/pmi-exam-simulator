using TestAPI.Enums;

namespace TestAPI.DTO.User
{
    public class UpdateUserStatusRequest
    {
        public AccountStatus Status { get; set; }
    }
}
