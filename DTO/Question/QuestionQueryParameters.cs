using System.Text.Json.Serialization;
using TestAPI.Entities;
using TestAPI.Models.Pagination;

namespace TestAPI.DTO.Question
{
    public class QuestionQueryParameters : PageParameters
    {
        [JsonPropertyName("domainId")]
        public Guid? DomainId { get; set; }

        [JsonPropertyName("questionType")]
        public QuestionType? QuestionType { get; set; }

        [JsonPropertyName("search")]
        public string? Search { get; set; }
    }
}
