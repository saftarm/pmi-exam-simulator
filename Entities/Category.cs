using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TestAPI.Entities
{
  public class Category : BaseEntity
  {
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    [Required]
    [MaxLength(500)]
    public string? Description { get; set; }
    [DefaultValue(0)]
    public int NumberOfExams { get; set; }
    public ICollection<Exam> Exams { get; set; } = [];
    public Category(string title, string description)
    {
      Title = title;
      Description = description;
    }
  }
}
