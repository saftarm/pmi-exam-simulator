namespace TestAPI.DTO.User;

public class UpdateProfileRequest
{
    public string DisplayName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
