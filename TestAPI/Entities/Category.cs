

namespace TestAPI.Entities
{
    public class Category
    {

        public int Id { get; set; }
        public string? Title { get; set; }

        public int NumberOfExams { get; set; }
        public ICollection<Exam>? Exams { get; set; }
    }
}