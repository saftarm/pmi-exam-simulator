using TestAPI.DTO;


namespace TestAPI.Services.Interfaces
{
    public interface IQuestionService
    {
        public Task<Guid> CreateAsync(CreateQuestionDto createQuestionDto);
        public Task CreateRangeAsync(List<CreateQuestionDto> questions);
        public Task<IEnumerable<QuestionDto>> GetAllAsync();
        public Task<QuestionDto> GetByIdAsync(Guid questionId);
        public Task DeleteRangeAsync(IEnumerable<Guid> questionIds);
        public Task DeleteQuestion(Guid questionId);
        public Task UpdateAsync(UpdateQuestionRequest request);

    }
}
