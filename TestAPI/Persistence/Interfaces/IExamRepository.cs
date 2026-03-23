using TestAPI.DTO;
using TestAPI.Entities;

namespace TestAPI.Persistence.Interfaces
{
    public interface IExamRepository
    {


        public IQueryable<Exam> GetAllAsync();

        public Task<ExamStatus> GetExamStatusByIdAsync(int id);

        public Task AddAsync(IEnumerable<Exam> exams);

        public Task DeleteAsync(int examId);

        public Task DeleteRangeAsync(IEnumerable<int> examIds);

        public Task UpdateAsync(Exam exam);

        public Task<Exam> GetByIdAsync(int id);


        public Task<IEnumerable<Exam>> GetAllById(ICollection<int> examIds);

        public Task<IEnumerable<Question>> GetQuestionsByExamIdAsync(int examId);

        // public Task AddQuestionToExamAsync(int questionId, Exam exam);

        public Task AddQuestionsToExamAsync(int examId, ICollection<Question> questions);



    }
}