using TestAPI.Entities;
using TestAPI.Enums;
using TestAPI.Models.Pagination;

namespace TestAPI.Persistence.Interfaces
{
  public interface IExamRepository
  {
    // Basic CRUD
    public Task<int> AddAsync(Exam newExam); // create Exam
    public Task UpdateAsync(Exam exam);


    // Queries

    public Task<IEnumerable<Exam>> GetPublishedPaginatedExamsAsync(PageParameters pageParameters);

    // Exam specific details
    public Task<ExamStatus?> GetExamStatusByIdAsync(Guid examId);
    public Task<int?> QueryNumberOfQuestionsByExamId(Guid examId);
    public Task<IEnumerable<Guid>> GetDomainIdsById(Guid examId);

    // Queries (AsNoTracking)
    public Task<Exam?> QueryExamsWithDomainsById(Guid examId);

    public Task<IEnumerable<Exam>> GetAllExams();
    public Task<Exam?> GetByIdAsync(Guid id);
    public IQueryable<Exam> GetAllAsync();
    public Task DeleteAsync(Guid examId);
    public Task DeleteRangeAsync(IEnumerable<Guid> examIds);
    public Task<IEnumerable<Exam>> GetAllByIds(ICollection<Guid> examIds);
    public Task<IEnumerable<Question>> GetQuestionsByExamIdAsync(Guid examId);
    public Task<IEnumerable<Exam>> GetPublishedExamsByCategoryIdAsync(Guid categoryId, PageParameters pageParameters, CancellationToken ct);

    // Checks for existence
    public Task<bool> ExamExistsByExamId(Guid examId);
    public Task<bool> ExamExistsByTitleAsync(string? title);
  }
}
