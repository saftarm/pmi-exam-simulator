using TestAPI.DTO;
using TestAPI.Entities;

namespace TestAPI.Persistence.Interfaces
{
    public interface IExamRepository
    {


        public Task<IEnumerable<Exam>> GetAllAsync();

        public void Add(Exam exam);

        public Task DeleteAsync(int examId);

        public Task<Exam?> GetByIdAsync(int id);

        public Task<IEnumerable<Question>> GetQuestionsByExamIdAsync(int examId);



        public Task AddQuestionToExamAsync(int questionId, Exam exam);
    }
}