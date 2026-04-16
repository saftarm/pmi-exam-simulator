using TestAPI.Entities;
using TestAPI.Models;

namespace TestAPI.Persistence.Interfaces
{
    public interface IExamRepository
    {
        public IQueryable<Exam> GetAllAsync();
        public Task<ExamStatus> GetExamStatusByIdAsync(Guid id);
        public Task AddAsync(IEnumerable<Exam> exams);
        public Task DeleteAsync(Guid examId);
        public Task DeleteRangeAsync(IEnumerable<Guid> examIds);
        public Task UpdateAsync(Exam exam);
        public Task<Exam?> GetByIdAsync(Guid id);
        public Task<IEnumerable<Guid>> GetDomainIdsById(Guid id);
        public Task<IEnumerable<Exam>> GetAllById(ICollection<Guid> examIds);
        public Task<IEnumerable<Question>> GetQuestionsByExamIdAsync(Guid examId);
        public Task AddQuestionsToExamAsync(Guid examId, ICollection<Question> questions);
        public Task<IEnumerable<Exam>?> GetPublishedExamsByCategoryIdAsync(Guid categoryId, PageParameters pageParameters, CancellationToken ct);
    }
}