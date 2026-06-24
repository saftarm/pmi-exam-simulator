namespace TestAPI.DTO.Exam.Requests
{
    public class BulkDeleteExamsRequest
    {
        public IEnumerable<Guid> ExamIds { get; set; } = [];
    }
}
