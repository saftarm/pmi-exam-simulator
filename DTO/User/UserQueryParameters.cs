using System.Text.Json.Serialization;
using TestAPI.Enums;
using TestAPI.Models.Pagination;

namespace TestAPI.DTO.User
{
    public class UserQueryParameters : PageParameters
    {
        [JsonPropertyName("search")]
        public string? Search { get; set; }

        [JsonPropertyName("role")]
        public UserRole? Role { get; set; }

        [JsonPropertyName("status")]
        public AccountStatus? Status { get; set; }
    }
}
