namespace TestAPI.DTO.Question
{
    public class BulkDeleteQuestionsRequest
    {
        public IEnumerable<Guid> QuestionIds { get; set; } = [];
    }
}
