using System.Text.Json.Serialization;

namespace TestAPI.Models.Pagination
{
  public class PageParameters
  {
    [JsonPropertyName("pageNumber")]
    public int PageNumber { get; set; } 
    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; } 
  }
}
