using TestAPI.Entities;

namespace TestAPI.Persistence.Interfaces
{
    public interface IUserExamResponseRepository
    {
        public Task<Guid> AddAsync(UserExamResponse response);
    }
}
