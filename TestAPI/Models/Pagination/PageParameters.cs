using System.Text.Json.Serialization;

namespace TestAPI.Models
{
    public class PageParameters
    {
        [JsonPropertyName("pageNumber")]

        public int PageNumber {get;set;} = 1;

        [JsonPropertyName("pageSize")]

        public int PageSize {get;set; } = 20;
    }
}
