namespace TestAPI.DTO.Auth.Requests
{
    public class RegisterUserRequest
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? DisplayName { get; set; }
        public string? Email { get; set; }
    }
}

