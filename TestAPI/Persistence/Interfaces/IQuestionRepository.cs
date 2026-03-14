using TestAPI.Entities;
using TestAPI.DTO;
namespace TestAPI.Persistence.Interfaces
{
    public interface IQuestionRepository
    {
        
        public Task<int> AddAsync(Question newQuestion);

        public Task<UpdateQuestionDto> UpdateAsync(int questionId, UpdateQuestionDto questionDto);

        public Task DeleteAsync(int questionId);

        public Task<IEnumerable<Question>> GetAllAsync();

        public Task<Question> GetByIdAsync(int questionId);
        
        // public Task<List<Exam>?> GetExamsByQuestionIdAsync (int questionId);



    }
}
