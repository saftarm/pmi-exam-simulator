using System;

namespace TestAPI.Entities
{
public class Domain
{
	public int Id { get; set; }

	public string? Title { get; set; }

	public string? Description { get; set; }

	public int Weight { get; set; }

	public int ExamId { get; set; }

	public Exam? Exam { get; set; }

	public ICollection<Question> Questions {get;set;} = new List<Question>();


}

}
