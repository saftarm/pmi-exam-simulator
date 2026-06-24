using System.Text.Json.Serialization;

namespace TestAPI.DTO
{
    public record FinishSessionRequest
    {
        [JsonPropertyName("sessionId")]
        public Guid SessionId { get; set; }

        [JsonPropertyName("sessionResponses")]
        public IEnumerable<UserExamResponseDto> SessionResponses { get; set; } = [];

    }
}

