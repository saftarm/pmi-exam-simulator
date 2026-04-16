using TestAPI.DTO;
using TestAPI.Entities;
namespace TestAPI.Persistence.Interfaces
{
    public interface IQuestionRepository
    { 
        public Task<Guid> AddAsync(Question newQuestion);
        public Task AddRangeAsync(IEnumerable<Question> questions);
        public Task UpdateAsync(Question question);
        public Task DeleteAsync(Guid questionId);
        public Task<IEnumerable<Question>> GetAllAsync();
        public Task<Question> GetByIdAsync(Guid questionId);
        public Task<ICollection<Question>> GetByIdsAsync(ICollection<Guid> questionIds);
        public Task<IEnumerable<AnswerOption>> GetAnswerOptionsByQuestionID(Guid questionId);
        public Task DeleteRangeAsync(IEnumerable<Guid> questionIds);
        public Task<IEnumerable<Question>> GetFixedAmountOfRandomQuestionsByDomainId(Guid domainId, int numberOfQuestions);
        public Task<int> GetNumberOfQuestionsByDomainId(Guid domainId);
        public Task<bool> ExistsAsync(Guid id, CancellationToken ct);

    }
}
