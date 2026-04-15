



using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.Entities;
using TestAPI.Exceptions;
using TestAPI.Persistence.Interfaces;


namespace TestAPI.Persistence.Implementation
{
    public class ExamAttemptRepository : IExamAttemptRepository
    { 

        private readonly ApplicationDbContext _context;

        public ExamAttemptRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ExamAttempt> GetByIdAsync(Guid examAttemptId)
        {
            var examAttempt = await _context.ExamAttempts
            .Include(ea => ea.UserExamResponses)
            .Include(ea => ea.Exam)
                .ThenInclude(e => e.Domains)
                    .ThenInclude(e => e.Questions)
            .FirstOrDefaultAsync(ea => ea.Id == examAttemptId);

            return examAttempt;
        }

        public async Task<IEnumerable<ExamAttempt>> GetAllAsync()
        {
            return await _context.ExamAttempts.ToListAsync();
        }

        public async Task<ExamAttempt> GetByUserId(Guid userId)
        {
            var examAttempt = await _context.ExamAttempts
            .Include(a => a.UserExamResponses)
            .FirstOrDefaultAsync(e => e.UserId == userId);

            if (examAttempt == null)
            {
                throw new Exception("Attempt not found");
            }
            return examAttempt;
        }

        public async Task<Guid> AddAsync(ExamAttempt examAttempt)
        {

            await _context.ExamAttempts.AddAsync(examAttempt);
            await _context.SaveChangesAsync();
            return examAttempt.Id;
        }

        public async Task<Guid> UpdateAsync(ExamAttempt updatedExamAttempt)
        {
            _context.ExamAttempts.Update(updatedExamAttempt);
            await _context.SaveChangesAsync();
            return updatedExamAttempt.Id;
        }
        public async Task DeleteAsync(Guid examAttemptId)
        {
            var examAttempt = await _context.ExamAttempts.FindAsync(examAttemptId);
            if (examAttempt == null)
            {
                throw new RecordNotFoundException("Attempt not found");
            }
            _context.ExamAttempts.Remove(examAttempt);
            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<UserExamResponse>> GetResponsesAsync(Guid examAttemptId)
        {
            var examAttempt = await _context.ExamAttempts
            .Include(r => r.UserExamResponses)
            .FirstOrDefaultAsync(a => a.Id == examAttemptId);

            if (examAttempt == null)
            {
                throw new RecordNotFoundException("Attemp Not Found");
            }

            if (examAttempt.UserExamResponses == null)
            {
                throw new Exception("Responses not found");
            }
            return examAttempt.UserExamResponses.ToList();
        }

        public async Task<IEnumerable<ExamAttempt>?> GetAttemptsByExamAndUserIdAsync(Guid userId, Guid examId, CancellationToken ct)
        {
            return await _context.ExamAttempts
                .Where(a => a.UserId == userId && a.ExamId == examId)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.ExamAttempts.AnyAsync(a => a.Id == id);
        }



        //public async Task<string?> GetExamTitleByExamIdAsync(Guid examId, CancellationToken ct)
        //{
        //    return await _context.ExamAttempts
        //        .Where(a => a.ExamId == examId)
        //        .Select(a => a.ExamTitle)
        //        .FirstOrDefaultAsync(ct);
        //}






    }
}


