using TestAPI.Entities;
using TestAPI.DTO;
namespace TestAPI.Persistence.Interfaces
{
    public interface IQuestionRepository
    {
        
        public Task<int> AddAsync(Question newQuestion);

        public Task AddRangeAsync(ICollection<Question> questions);

        public Task<UpdateQuestionDto> UpdateAsync(int questionId, UpdateQuestionDto questionDto);

        public Task DeleteAsync(int questionId);

        public Task<IEnumerable<Question>> GetAllAsync();

        public Task<Question> GetByIdAsync(int questionId);

        public Task<ICollection<Question>> GetByIdsAsync(ICollection<int> questionIds);
        
        public Task<IEnumerable<AnswerOption>> GetAnswerOptionsByQuestionID (int questionId);

        public Task DeleteRangeAsync(IEnumerable<int> questionIds);




    }
}
