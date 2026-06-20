namespace TestAPI.DTO.User
{
  public class UserStatsDto
  {
    public int TotalCount { get; set; }
    public Dictionary<string, int> ByRole { get; set; } = [];
    public Dictionary<string, int> ByStatus { get; set; } = [];
  }
}
