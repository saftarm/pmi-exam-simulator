using TestAPI.DTO.Question;
using TestAPI.ResultPattern;


namespace TestAPI.Services.Interfaces
{
    public interface IQuestionService
    {
        // Basic CRUD
        Task<Result> CreateQuestionAsync(CreateQuestionDto createQuestionDto);
        Task<Result> DeleteRangeAsync(IEnumerable<Guid> questionIds);
        Task<Result> UpdateAsync(UpdateQuestionRequest request);
        Task DeleteQuestionAsync(Guid questionId);

        // Details 
        public Task<QuestionDto> GetByIdAsync(Guid questionId);

    }
}
